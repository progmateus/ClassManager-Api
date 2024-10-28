using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class CreateSubscriptionHandler : Notifiable,
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
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;

  public CreateSubscriptionHandler(
    ISubscriptionRepository subscriptionRepository,
    IUsersRolesRepository usersRolesRepository,
    IRoleRepository roleRepository,
    IStudentsClassesRepository studentsClassesRepository,
    IClassRepository classRepository,
    ITenantPlanRepository tenantPlanrepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IUserRepository userRepository,
    IInvoiceRepository invoiceRepository,
    IStripeCustomerRepository stripeCustomerRepository

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
    _invoiceRepository = invoiceRepository;
    _stripeCustomerRepository = stripeCustomerRepository;

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
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    Guid userId = loggedUserId;

    if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      userId = command.UserId;
    }

    var subscriptionAlreadyActive = await _subscriptionRepository.HasActiveSubscription(userId, tenantId, new CancellationToken());

    if (subscriptionAlreadyActive)
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
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

    var userRoleAlreadyExists = await _usersRolesRepository.HasAnyRoleAsync(userId, tenantId, ["student"], new CancellationToken());

    if (!userRoleAlreadyExists)
    {
      var userRole = new UsersRoles(userId, role.Id, tenantId);

      await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());
    }

    DateTime lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

    var user = await _userRepository.GetByIdAsync(userId, default);

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var stripeCustomerEntity = await _stripeCustomerRepository.FindByUserIdAndTenantIdAndType(user.Id, tenantId, EStripeCustomerType.USER, new CancellationToken());

    if (stripeCustomerEntity is null)
    {
      var stripeCustomerCreated = _paymentService.CreateCustomer(user.Name.ToString(), user.Email.ToString(), tenantPlan.Tenant.StripeAccountId);
      stripeCustomerEntity = new StripeCustomer(user.Id, tenantId, stripeCustomerCreated.Id, EStripeCustomerType.USER);
      user.StripeCustomers.Add(stripeCustomerEntity);
      await _userRepository.UpdateAsync(user, default);
    }

    var stripeSubscriptionCreated = _paymentService.CreateSubscription(tenantId, tenantPlan.StripePriceId, stripeCustomerEntity.StripeCustomerId, tenantPlan.Tenant.StripeAccountId);

    var stripeInvoice = _paymentService.CreateInvoice(tenantId, stripeCustomerEntity.StripeCustomerId, stripeSubscriptionCreated.Id, tenantPlan.Tenant.StripeAccountId);

    var subscription = new Subscription(userId, command.TenantPlanId, tenantId, stripeSubscriptionCreated.Id, lastDayOfMonth);

    /* var invoice = new Invoice(userId, tenantPlan.Id, subscription.Id, null, tenantPlan.TenantId, tenantPlan.Price, EInvoiceTargetType.USER, EInvoiceType.USER_SUBSCRIPTION); */

    /* invoice.SetStripeInformations(stripeInvoice.Id, stripeInvoice.HostedInvoiceUrl, stripeInvoice.Number); */

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());
    /* await _invoiceRepository.CreateAsync(invoice, new CancellationToken()); */

    var studentclass = new StudentsClasses(userId, command.ClassId);

    await _studentsClassesRepository.DeleteByUserIdAndtenantId(tenantId, userId, new CancellationToken());

    await _studentsClassesRepository.CreateAsync(studentclass, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}