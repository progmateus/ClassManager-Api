using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class CreateSubscriptionHandler : Notifiable,
  IActionHandler<CreateSubscriptionCommand>
{
  private ISubscriptionRepository _subscriptionRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private IRoleRepository _roleRepository;
  private IStudentsClassesRepository _studentsClassesRepository;
  private IClassRepository _classRepository;
  public CreateSubscriptionHandler(ISubscriptionRepository subscriptionRepository, IUsersRolesRepository usersRolesRepository, IRoleRepository roleRepository, IStudentsClassesRepository studentsClassesRepository, IClassRepository classRepository)
  {
    _subscriptionRepository = subscriptionRepository;
    _usersRolesRepository = usersRolesRepository;
    _roleRepository = roleRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _classRepository = classRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, CreateSubscriptionCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_CREATED", null, command.Notifications);
    }

    var role = await _roleRepository.GetByNameAsync("student", new CancellationToken());

    if (role is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    var classExists = await _classRepository.GetByIdAsync(command.ClassId, new CancellationToken());

    if (classExists is null)
    {
      return new CommandResult(false, "CLASS_NOT_FOUND", null, null, 404);
    }

    var subscriptionAlreadyActive = await _subscriptionRepository.HasActiveSubscription(command.UserId, tenantId, new CancellationToken());

    if (subscriptionAlreadyActive)
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var userRoleAlreadyExists = await _usersRolesRepository.VerifyRoleExistsAsync(command.UserId, tenantId, "student", new CancellationToken());

    if (!userRoleAlreadyExists)
    {
      var userRole = new UsersRoles(command.UserId, role.Id, tenantId);

      await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());
    }

    DateTime lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

    var subscription = new Subscription(command.UserId, command.TenantPlanId, lastDayOfMonth);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    var studentclass = new StudentsClasses(command.UserId, command.ClassId);

    await _studentsClassesRepository.CreateAsync(studentclass, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CREATED", subscription, null, 201);
  }
}