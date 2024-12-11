using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class UpdateSubscriptionStatusHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private ITenantPlanRepository _tenantPlanrepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly ITenantRepository _tenantRepository;

  public UpdateSubscriptionStatusHandler(ISubscriptionRepository subscriptionRepository,
  ITenantPlanRepository tenantPlanrepository,
  IAccessControlService accessControlService,
  IPaymentService paymentService,
  ITenantRepository tenantRepository

  )
  {
    _subscriptionRepository = subscriptionRepository;
    _tenantPlanrepository = tenantPlanrepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _tenantRepository = tenantRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId, UpdateSubscriptionCommand command)
  {

    if (!command.Status.HasValue)
    {
      return new CommandResult(false, "ERR_INVALID_STATUS", null, null, 400);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var istenantAdmin = await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]);

    var subscription = await _subscriptionRepository.FindByIdAndTenantIdAsync(subscriptionId, tenantId, new CancellationToken());

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());


    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (subscription is null)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }

    var userLatestSubscription = await _subscriptionRepository.FindUserLatestSubscription(tenantId, subscription.UserId, new CancellationToken());

    if (userLatestSubscription is not null && !userLatestSubscription.Id.Equals(subscription.Id))
    {
      return new CommandResult(false, "ERR_ONLY_UPDATE_THE_LATEST_SUBSCRIPTION", null, null, 409);
    }

    if (!subscription.UserId.Equals(loggedUserId) && !istenantAdmin)
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 404);
    }

    if (command.Status.Value == subscription.Status)
    {
      return new CommandResult(false, "ERR_INVALID_STATUS", null, null, 400);
    }

    if (subscription.Status != ESubscriptionStatus.ACTIVE && subscription.Status != ESubscriptionStatus.PAUSED)
    {
      return new CommandResult(false, "ERR_INVALID_STATUS", null, null, 400);
    }

    if (subscription.Status == ESubscriptionStatus.CANCELED)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_CANCELED", null, null, 400);
    }

    if (command.Status == ESubscriptionStatus.ACTIVE)
    {
      _paymentService.ResumeSubscription(subscription.StripeSubscriptionId, tenant.StripeAccountId);
    }

    else if (command.Status == ESubscriptionStatus.CANCELED)
    {
      _paymentService.CancelSubscription(subscription.StripeSubscriptionId, tenant.StripeAccountId);
    }

    return new CommandResult(true, "SUBSCRIPTION_UPDATED", subscription, null, 200);
  }
}