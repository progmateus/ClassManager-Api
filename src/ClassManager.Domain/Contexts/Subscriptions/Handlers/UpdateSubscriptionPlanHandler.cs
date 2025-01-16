using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Subscriptions.Handlers;

public class UpdateSubscriptionPlanHandler : Notifiable
{
  private ISubscriptionRepository _subscriptionRepository;
  private IPlanRepository _planRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly ITenantRepository _tenantRepository;
  private ITenantPlanRepository _tenantPlanrepository;


  public UpdateSubscriptionPlanHandler(ISubscriptionRepository subscriptionRepository,
    IPlanRepository planRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    ITenantRepository tenantRepository,
    ITenantPlanRepository tenantPlanrepository


  )
  {
    _subscriptionRepository = subscriptionRepository;
    _planRepository = planRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _tenantRepository = tenantRepository;
    _tenantPlanrepository = tenantPlanrepository;


  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId, UpdateSubscriptionCommand command)
  {

    if (!command.TenantPlanId.HasValue)
    {
      return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
    }


    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    var subscription = await _subscriptionRepository.FindByIdAndTenantIdAsync(subscriptionId, tenantId, new CancellationToken());

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());


    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    if (subscription is null || subscription.TargetType != ETargetType.TENANT)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", null, null, 404);
    }


    if (subscription.TargetType == ETargetType.TENANT)
    {
      if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 404);
      }

      var plan = await _planRepository.GetByIdAsync(command.TenantPlanId.Value, new CancellationToken());

      if (plan is null)
      {
        return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
      }

      if (!subscription.StripeScheduleSubscriptionNextPlanId.IsNullOrEmpty())
      {
        _paymentService.CancelSubscriptionSchedule(subscription.StripeScheduleSubscriptionNextPlanId, null);
      }

      var subscriptionSchedule = _paymentService.ScheduleUpdateSubscriptionPlan(subscription.StripeSubscriptionId, plan.StripePriceId, null);

      subscription.SetPlan(plan.Id);
      subscription.SetStripeScheduleSubscriptionNextPlanId(subscriptionSchedule.Id);
    }
    else
    {
      if (!subscription.UserId.Equals(loggedUserId) && !await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
      {
        return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 404);
      }

      var tenantPlan = await _tenantPlanrepository.FindByIdAndTenantIdAsync(command.TenantPlanId.Value, tenantId, new CancellationToken());

      if (tenantPlan is null)
      {
        return new CommandResult(false, "ERR_TENANT_PLAN_NOT_FOUND", null, null, 404);
      }

      _paymentService.UpdateSubscriptionPlan(tenant.Id, subscription.Id, subscription.StripeSubscriptionPriceItemId, tenantPlan.StripePriceId, tenant.StripeAccountId);
      /* subscription.SetTenantPlan(tenantPlan.Id); */
    }

    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_UPDATED", subscription, null, 200);
  }
}