using AutoMapper;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Subscriptions.Users.Handlers;

public class RefreshUserSubscriptionHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly IMapper _mapper;
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;

  public RefreshUserSubscriptionHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IMapper mapper,
    ITenantPlanRepository tenantPlanRepository,
    ISubscriptionRepository subscriptionRepository,
    IStripeCustomerRepository stripeCustomerRepository

    )
  {
    _tenantRepository = tenantRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _mapper = mapper;
    _tenantPlanRepository = tenantPlanRepository;
    _subscriptionRepository = subscriptionRepository;
    _stripeCustomerRepository = stripeCustomerRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid subscriptionId)
  {

    var tenant = await _tenantRepository.GetByIdAsync(tenantId, new CancellationToken());

    if (tenant is null)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", new { }, null, 404);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    Guid targetUserId = loggedUserId;

    var targetsubscription = await _subscriptionRepository.FindByIdAndTenantIdAsync(subscriptionId, tenantId, new CancellationToken());

    if (targetsubscription is null || targetsubscription.TargetType != ETargetType.USER)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_FOUND", new { }, null, 404);
    }

    if (targetsubscription.Status != ESubscriptionStatus.INCOMPLETE_EXPIRED)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_INCOMPLETE_EXPIRED", new { }, null, 404);
    }

    if (targetsubscription.UserId.HasValue && targetsubscription.UserId != Guid.Empty)
    {
      if (await _accessControlService.CheckParameterUserIdPermission(tenantId, loggedUserId, targetsubscription.UserId))
      {
        targetUserId = targetsubscription.UserId.Value;
      }
      else
      {
        return new CommandResult(false, "ADMIN_ROLE_NOT_FOUND", null, null, 409);
      }
    }

    var subscriptionsAlreadyExists = await _subscriptionRepository.GetSubscriptionsByStatus(targetUserId, tenantId, [ESubscriptionStatus.ACTIVE, ESubscriptionStatus.UNPAID, ESubscriptionStatus.PAST_DUE, ESubscriptionStatus.INCOMPLETE, ESubscriptionStatus.PAUSED], ETargetType.TENANT);

    if (subscriptionsAlreadyExists.Any(x => x.Status == ESubscriptionStatus.ACTIVE || x.Status == ESubscriptionStatus.INCOMPLETE || x.Status != ESubscriptionStatus.PAUSED))
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    if (subscriptionsAlreadyExists.Any(x => x.Status == ESubscriptionStatus.UNPAID || x.Status == ESubscriptionStatus.PAST_DUE))
    {
      return new CommandResult(false, "UNPAID_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var tenantPlan = await _tenantPlanRepository.FindAsync(x => x.TenantId == tenantId && x.Id == targetsubscription.TenantPlanId, [x => x.Tenant]);

    if (tenantPlan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    var stripeCustomerEntity = await _stripeCustomerRepository.FindByUserIdAndTenantIdAndType(targetUserId, tenantId, ETargetType.USER, new CancellationToken());

    if (stripeCustomerEntity is null)
    {
      return new CommandResult(false, "ERR_CUSTOMER_NOT_FOUND", null, null, 404);
    }

    var subscription = new Subscription(targetsubscription.TenantId, tenantPlan.Id, targetUserId);

    var stripeSubscription = _paymentService.CreateSubscription(subscription.Id, subscription.UserId, tenantId, tenantPlan.StripePriceId, stripeCustomerEntity.StripeCustomerId, ETargetType.USER, tenantPlan.Tenant.StripeAccountId);

    var stripeSubscriptionPriceItem = stripeSubscription.Items.Data.FirstOrDefault(x => x.Object == "subscription_item");

    if (stripeSubscriptionPriceItem is not null)
    {
      subscription.SetStripeSubscriptionPriceItemId(stripeSubscriptionPriceItem.Id);
    }

    subscription.SetStripeSubscriptionId(stripeSubscription.Id);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(null, subscription.UserId, tenantPlan.TenantId, stripeCustomerEntity.StripeCustomerId, stripeSubscription.Id, tenantPlan.Tenant.StripeAccountId);

    return new CommandResult(true, "SUBSCRIPTION_REFRESHED", _mapper.Map<SubscriptionViewModel>(subscription), null, 200);
  }
}
