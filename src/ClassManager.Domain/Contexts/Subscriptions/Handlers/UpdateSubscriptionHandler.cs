using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class UpdateSubscriptionHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private ITenantPlanRepository _tenantPlanrepository;
  private readonly IAccessControlService _accessControlService;

  public UpdateSubscriptionHandler(ISubscriptionRepository subscriptionRepository,
  ITenantPlanRepository tenantPlanrepository,
  IAccessControlService accessControlService

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _tenantPlanrepository = tenantPlanrepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId, UpdateSubscriptionCommand command)
  {


    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var istenantAdmin = await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin");

    var subscription = await _subscriptionRepository.FindByIdAsync(subscriptionId, tenantId, new CancellationToken());

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    if (!subscription.UserId.Equals(loggedUserId) && !istenantAdmin)
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 404);
    }

    if (command.Status.HasValue)
    {

      if (subscription.Status == ESubscriptionStatus.CANCELED)
      {
        return new CommandResult(false, "ERR_SUBSCRIPTION_INACTIVE", null, null, 400);
      }
      subscription.ChangeStatus(command.Status.Value);
    }

    if (command.TenantPlanId.HasValue && istenantAdmin)
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