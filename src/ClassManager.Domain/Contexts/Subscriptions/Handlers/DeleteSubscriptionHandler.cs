using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class DeleteSubscriptionHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IStudentsClassesRepository _studentsClassesRepository;
  private IUsersRolesRepository _usersRolesrepository;
  public DeleteSubscriptionHandler(ISubscriptionRepository subscriptionRepository, IUsersRolesRepository usersRolesrepository, IStudentsClassesRepository studentsClassesRepository)
  {
    _subscriptionRepository = subscriptionRepository;
    _usersRolesrepository = usersRolesrepository;
    _studentsClassesRepository = studentsClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid subscriptionId, Guid userId)
  {
    var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, tenantId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (subscription.TenantId != tenantId || subscription.UserId != userId)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_INACTIVE", null, null, 409);
    }

    var usersClassesFound = await _studentsClassesRepository.ListByUserOrClassOrTenantAsync([userId], [tenantId], null);

    var userRoles = await _usersRolesrepository.GetStudentsRolesByUserIdAndTenantId(tenantId, userId, new CancellationToken());

    subscription.ChangeStatus(ESubscriptionStatus.CANCELED);

    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());

    await _studentsClassesRepository.DeleteRangeAsync(usersClassesFound, new CancellationToken());

    await _usersRolesrepository.DeleteRangeAsync(userRoles, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_CANCELED", subscription, null, 204);
  }
}