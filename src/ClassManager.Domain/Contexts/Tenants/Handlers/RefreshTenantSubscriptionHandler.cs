using AutoMapper;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
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

  public RefreshTenantSubscriptionHandler(
    ITenantRepository tenantRepository,
    IAccessControlService accessControlService,
    IPaymentService paymentService,
    IMapper mapper,
    IPlanRepository planRepository

    )
  {
    _tenantRepository = tenantRepository;
    _accessControlService = accessControlService;
    _paymentService = paymentService;
    _mapper = mapper;
    _planRepository = planRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    var tenant = await _tenantRepository.FindAsync(x => x.Id == tenantId, [x => x.StripeCustomers.Where(sc => sc.Type == EStripeCustomerType.TENANT && sc.TenantId == tenantId)]);

    if (tenant is null || tenant.StripeCustomers.Count < 1)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", new { }, null, 404);
    }

    /* if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", new { }, null);
    } */

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", new { }, null, 403);
    }

    if (tenant.SubscriptionStatus != ESubscriptionStatus.INCOMPLETE_EXPIRED)
    {
      return new CommandResult(false, "ERR_SUBSCRIPTION_NOT_EXPIRED", new { }, null, 409);
    }

    var plan = await _planRepository.GetByIdAsync(tenant.PlanId, default);

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", new { }, null, 404);
    }

    var stripeSubscription = _paymentService.CreateSubscription(null, null, tenantId, plan.StripePriceId, tenant.StripeCustomers[0].StripeCustomerId, ETargetType.TENANT, null);

    tenant.SetSubscriptionStatus(ESubscriptionStatus.INCOMPLETE);

    tenant.SetStripeInformations(null, null, stripeSubscription.Id);

    await _tenantRepository.UpdateAsync(tenant, new CancellationToken());

    return new CommandResult(true, "SUBSCRIPTION_REFRESHED", _mapper.Map<TenantViewModel>(tenant), null, 200);
  }
}
