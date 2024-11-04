using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeSubscriptionWebhookHandler
{
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IPaymentService _paymentService;

  public UpdateStripeSubscriptionWebhookHandler(
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

    /* var tenantPlan = await _tenantPlanRepository.FindByStripePriceId(stripeSubscription.Items.Data[0].Plan.Id);

    if (tenantPlan is null)
    {
      return;
    } */

    var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

    if (subscription is null)
    {
      return;
    }

    var enabledStatus = new List<string> {
      "paused",
      "canceled",
      "active",
    };

    if (!enabledStatus.Contains(stripeSubscription.Status))
    {
      return;
    }

    var status = stripeSubscription.Status == "paused" ? ESubscriptionStatus.PAUSED : stripeSubscription.Status == "canceled" ? ESubscriptionStatus.CANCELED : ESubscriptionStatus.ACTIVE;

    subscription.ChangeStatus(status);
    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
  }
}
