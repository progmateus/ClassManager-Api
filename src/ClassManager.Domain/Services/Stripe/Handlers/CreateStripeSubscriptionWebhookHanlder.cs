using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Helpers;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class CreateStripeSubscriptionWebhookHandler
{
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IPaymentService _paymentService;

  public CreateStripeSubscriptionWebhookHandler(
    IStripeCustomerRepository stripeCustomerRepository,
    ISubscriptionRepository subscriptionRepository,
    ITenantPlanRepository tenantPlanRepository,
    IPaymentService paymentService

    )
  {
    _stripeCustomerRepository = stripeCustomerRepository;
    _subscriptionRepository = subscriptionRepository;
    _tenantPlanRepository = tenantPlanRepository;
    _paymentService = paymentService;
  }
  public async Task Handle(Subscription? stripeSubscription)
  {

    if (stripeSubscription is null)
    {
      return;
    }

    var customer = await _stripeCustomerRepository.FindByCustomerId(stripeSubscription.CustomerId, default);

    if (customer is null || customer.Type != EStripeCustomerType.USER)
    {
      return;
    }

    var tenantPlan = await _tenantPlanRepository.FindByStripePriceId(stripeSubscription.Items.Data[0].Plan.Id);

    if (tenantPlan is null)
    {
      return;
    }

    var subscription = new Contexts.Subscriptions.Entities.Subscription(customer.UserId, tenantPlan.Id, tenantPlan.TenantId, stripeSubscription.Id, stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);

    await _subscriptionRepository.CreateAsync(subscription, new CancellationToken());

    _paymentService.CreateInvoice(tenantPlan.TenantId, customer.StripeCustomerId, stripeSubscription.Id, tenantPlan.Tenant.StripeAccountId);
  }
}
