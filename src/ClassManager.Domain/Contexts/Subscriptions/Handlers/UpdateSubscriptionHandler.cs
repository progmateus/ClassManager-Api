using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class UpdateSubscriptionHandler : Notifiable,
  ITenantActionHandler<UpdateSubscriptionCommand>
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
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_CREATED", null, command.Notifications);
    }

    var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    subscription.ChangeStatus(command.Status);

    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_UPDATED", subscription, null, 200);
  }
}