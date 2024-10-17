using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
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
  private readonly IStripeService _stripeService;
  private readonly IUserRepository _userRepository;

  public CreateSubscriptionHandler(
    ISubscriptionRepository subscriptionRepository,
    IUsersRolesRepository usersRolesRepository,
    IRoleRepository roleRepository,
    IStudentsClassesRepository studentsClassesRepository,
    IClassRepository classRepository,
    ITenantPlanRepository tenantPlanrepository,
    IAccessControlService accessControlService,
    IStripeService stripeService,
    IUserRepository userRepository

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _usersRolesRepository = usersRolesRepository;
    _roleRepository = roleRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _classRepository = classRepository;
    _tenantPlanRepository = tenantPlanrepository;
    _accessControlService = accessControlService;
    _stripeService = stripeService;
    _userRepository = userRepository;

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

    var subscription = new Subscription(userId, command.TenantPlanId, tenantId, lastDayOfMonth);

    var user = await _userRepository.GetByIdAsync(userId, default);

    if (user is null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    if (user.StripeCustomerId.IsNullOrEmpty())
    {
      var stripeCustomer = _stripeService.CreateCustomer(user.Name.ToString(), user.Email.ToString());
      user.SetStripeCustomerId(stripeCustomer.Id);
      await _userRepository.UpdateAsync(user, default);
    }

    _stripeService.CreateSubscription(tenantId, tenantPlan.StripePriceId, user.StripeCustomerId);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    var studentclass = new StudentsClasses(userId, command.ClassId);

    await _studentsClassesRepository.DeleteByUserIdAndtenantId(tenantId, userId, new CancellationToken());

    await _studentsClassesRepository.CreateAsync(studentclass, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}