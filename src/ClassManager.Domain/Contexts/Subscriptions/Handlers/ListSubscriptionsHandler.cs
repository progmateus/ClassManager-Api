using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class ListSubscriptionsHandler : Notifiable,
  ITenantListHandler
{
  private ITenantRepository _tenantRepository;
  private ISubscriptionRepository _subscriptionRepository;
  public ListSubscriptionsHandler(ISubscriptionRepository subscriptionRepository, ITenantRepository tenantRepository, IUsersRolesRepository usersRolesRepository)
  {
    _tenantRepository = tenantRepository;
    _subscriptionRepository = subscriptionRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId)
  {

    var tenantExists = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var subscriptions = await _subscriptionRepository.ListByTenantIdAsync(tenantId, new CancellationToken());

    return new CommandResult(false, "SUBSCRIPTIONS_LISTED", subscriptions, null, 200);
  }
}