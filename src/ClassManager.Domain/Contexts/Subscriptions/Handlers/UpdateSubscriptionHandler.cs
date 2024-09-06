using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class UpdateSubscriptionHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private ITenantPlanRepository _tenantPlanrepository;
  public UpdateSubscriptionHandler(ISubscriptionRepository subscriptionRepository, ITenantPlanRepository tenantPlanrepository)
  {
    _subscriptionRepository = subscriptionRepository;
    _tenantPlanrepository = tenantPlanrepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid subscriptionId, UpdateSubscriptionCommand command)
  {
    var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, tenantId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (command.Status.HasValue)
    {
      subscription.ChangeStatus(command.Status.Value);

    }

    if (command.TenantPlanId.HasValue)
    {
      var tenantPlan = await _tenantPlanrepository.GetByIdAsync(command.TenantPlanId.Value, new CancellationToken());

      if (tenantPlan is null)
      {
        return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
      }
      subscription.ChangePlan(command.TenantPlanId.Value);
    }

    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_UPDATED", subscription, null, 200);
  }
}