using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class DeleteSubscriptionHandler : Notifiable, ITenantDeleteAction
{
  private ISubscriptionRepository _subscriptionRepository;
  private IStudentsClassesRepository _studentsClassesRepository;
  private IUsersRolesRepository _usersRolesrepository;
  private readonly IAccessControlService _accessControlService;

  public DeleteSubscriptionHandler(ISubscriptionRepository subscriptionRepository,
  IUsersRolesRepository usersRolesrepository,
  IStudentsClassesRepository studentsClassesRepository,
  IAccessControlService accessControlService

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _usersRolesrepository = usersRolesrepository;
    _studentsClassesRepository = studentsClassesRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var subscription = await _subscriptionRepository.FindByIdAndTenantIdAsync(subscriptionId, tenantId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (!subscription.UserId.Equals(loggedUserId) && !await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_INACTIVE", null, null, 409);
    }

    var usersClassesFound = await _studentsClassesRepository.ListByUserOrClassAndTenantAsync([subscription.UserId.Value], [tenantId], []);

    var userRoles = await _usersRolesrepository.GetStudentsRolesByUserIdAndTenantId(tenantId, subscription.UserId.Value, new CancellationToken());

    subscription.SetStatus(ESubscriptionStatus.CANCELED);

    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());

    await _studentsClassesRepository.DeleteRangeAsync(usersClassesFound, new CancellationToken());

    await _usersRolesrepository.DeleteRangeAsync(userRoles, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CANCELED", subscription, null, 204);
  }
}