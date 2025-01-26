using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers.Users;

public class CreateUserSubscriptionHandler : Notifiable,
  ITenantHandler<CreateSubscriptionCommand>
{
  private ISubscriptionRepository _subscriptionRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private IRoleRepository _roleRepository;
  private IStudentsClassesRepository _studentsClassesRepository;
  private IClassRepository _classRepository;
  private ITenantPlanRepository _tenantPlanRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly IUserRepository _userRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly IInvoiceRepository _invoiceRepository;

  public CreateUserSubscriptionHandler(
    ISubscriptionRepository subscriptionRepository,
    IUsersRolesRepository usersRolesRepository,
    IRoleRepository roleRepository,
    IStudentsClassesRepository studentsClassesRepository,
    IClassRepository classRepository,
    ITenantPlanRepository tenantPlanrepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IUserRepository userRepository,
    IStripeCustomerRepository stripeCustomerRepository,
    IInvoiceRepository invoiceRepository

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _usersRolesRepository = usersRolesRepository;
    _roleRepository = roleRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _classRepository = classRepository;
    _tenantPlanRepository = tenantPlanrepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _userRepository = userRepository;
    _stripeCustomerRepository = stripeCustomerRepository;
    _invoiceRepository = invoiceRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateSubscriptionCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    Guid targetUserId = loggedUserId;

    if (command.UserId.HasValue && command.UserId != Guid.Empty)
    {
      if (await _accessControlService.CheckParameterUserIdPermission(tenantId, loggedUserId, command.UserId))
      {
        targetUserId = command.UserId.Value;
      }
      else
      {
        return new CommandResult(false, "ADMIN_ROLE_NOT_FOUND", null, null, 409);
      }
    }

    var subscriptionsAlreadyExists = await _subscriptionRepository.GetSubscriptionsByStatus(targetUserId, tenantId, [ESubscriptionStatus.INCOMPLETE, ESubscriptionStatus.ACTIVE, ESubscriptionStatus.UNPAID, ESubscriptionStatus.PAST_DUE], ETargetType.USER);

    if (subscriptionsAlreadyExists.Any(x => x.Status == ESubscriptionStatus.ACTIVE || x.Status == ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    if (subscriptionsAlreadyExists.Any(x => x.Status != ESubscriptionStatus.ACTIVE && x.Status != ESubscriptionStatus.CANCELED && x.Status != ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "UNPAID_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var hasSubscriptionUnpaidInvoice = await _invoiceRepository.HasSubscriptionUnpaidInvoice(tenantId, targetUserId, ETargetType.USER, new CancellationToken());

    if (hasSubscriptionUnpaidInvoice)
    {
      return new CommandResult(false, "UNPAID_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var role = await _roleRepository.GetByNameAsync("student", new CancellationToken());

    if (role is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, command.ClassId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var tenantPlan = await _tenantPlanRepository.FindAsync(x => x.TenantId == tenantId && x.Id == command.TenantPlanId, [x => x.Tenant]);

    if (tenantPlan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var userRoleAlreadyExists = await _usersRolesRepository.HasAnyRoleAsync(targetUserId, tenantId, ["student"], new CancellationToken());

    if (!userRoleAlreadyExists)
    {
      var userRole = new UsersRoles(targetUserId, role.Id, tenantId);

      await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());
    }

    /* DateTime lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)); */

    var user = await _userRepository.GetByIdAsync(targetUserId, default);

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var stripeCustomerEntity = await _stripeCustomerRepository.FindByUserIdAndTenantIdAndType(user.Id, tenantId, ETargetType.USER, new CancellationToken());

    if (stripeCustomerEntity is null)
    {
      var stripeCustomerCreated = _paymentService.CreateCustomer(user.Name.ToString(), user.Email.ToString(), tenantPlan.Tenant.StripeAccountId);
      stripeCustomerEntity = new StripeCustomer(stripeCustomerCreated.Id, tenantId, user.Id);
      await _stripeCustomerRepository.CreateAsync(stripeCustomerEntity, new CancellationToken());
    }

    var subscription = new Subscription(tenantPlan.TenantId, tenantPlan.Id, stripeCustomerEntity.UserId);

    var stripeSubscription = _paymentService.CreateSubscription(subscription.Id, subscription.UserId, tenantId, tenantPlan.StripePriceId, stripeCustomerEntity.StripeCustomerId, ETargetType.USER, tenantPlan.Tenant.StripeAccountId);

    var stripeSubscriptionPriceItem = stripeSubscription.Items.Data.FirstOrDefault(x => x.Object == "subscription_item");

    if (stripeSubscriptionPriceItem is not null)
    {
      subscription.SetStripeSubscriptionPriceItemId(stripeSubscriptionPriceItem.Id);
    }

    subscription.SetStripeSubscriptionId(stripeSubscription.Id);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(null, subscription.UserId, tenantPlan.TenantId, stripeCustomerEntity.StripeCustomerId, stripeSubscription.Id, tenantPlan.Tenant.StripeAccountId);

    var studentclass = new StudentsClasses(targetUserId, command.ClassId);

    await _studentsClassesRepository.DeleteByUserIdAndtenantId(tenantId, [targetUserId], new CancellationToken());

    await _studentsClassesRepository.CreateAsync(studentclass, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}