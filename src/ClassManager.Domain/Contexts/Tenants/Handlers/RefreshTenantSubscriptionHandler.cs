using AutoMapper;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
namespace ClassManager.Domain.Contexts.Tenants.Handlers;

public class RefreshTenantSubscriptionHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IPaymentService _paymentService;
  private readonly IMapper _mapper;
  private readonly IPlanRepository _planRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;

  public RefreshTenantSubscriptionHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IMapper mapper,
    IPlanRepository planRepository,
    ISubscriptionRepository subscriptionRepository

    )
  {
    _tenantRepository = tenantRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _mapper = mapper;
    _planRepository = planRepository;
    _subscriptionRepository = subscriptionRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    var tenant = await _tenantRepository.FindAsync(x => x.Id == tenantId, [x => x.StripeCustomers.Where(sc => sc.TargetType == ETargetType.TENANT && sc.TenantId == tenantId)]);

    if (tenant is null || tenant.StripeCustomers.Count < 1)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", new { }, null, 404);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", new { }, null, 403);
    }

    var subscriptionsAlreadyExists = await _subscriptionRepository.GetSubscriptionsByStatus(null, tenantId, [ESubscriptionStatus.ACTIVE, ESubscriptionStatus.UNPAID, ESubscriptionStatus.PAST_DUE, ESubscriptionStatus.INCOMPLETE], ETargetType.TENANT);

    if (subscriptionsAlreadyExists.Any(x => x.Status == ESubscriptionStatus.ACTIVE || x.Status == ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "ACTIVE_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    if (subscriptionsAlreadyExists.Any(x => x.Status != ESubscriptionStatus.ACTIVE && x.Status != ESubscriptionStatus.CANCELED && x.Status != ESubscriptionStatus.INCOMPLETE))
    {
      return new CommandResult(false, "UNPAID_SUBSCRIPTION_ALREADY_EXISTS", null, null, 409);
    }

    var plan = await _planRepository.GetByIdAsync(tenant.PlanId, default);

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", new { }, null, 404);
    }

    var subscription = new Subscription(tenant.Id, plan.Id);

    var stripeSubscription = _paymentService.CreateSubscription(subscription.Id, null, tenant.Id, plan.StripePriceId, tenant.StripeCustomerId, ETargetType.TENANT, null);


    subscription.SetStripeSubscriptionId(stripeSubscription.Id);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(null, null, tenant.Id, tenant.StripeCustomerId, stripeSubscription.Id, tenant.StripeAccountId);

    return new CommandResult(true, "SUBSCRIPTION_REFRESHED", _mapper.Map<SubscriptionViewModel>(subscription), null, 200);
  }
}
