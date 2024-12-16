using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeSubscriptionWebhookHandler
{
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly ITenantRepository _tenantRepository;

  public UpdateStripeSubscriptionWebhookHandler(
    IStripeCustomerRepository stripeCustomerRepository,
    ISubscriptionRepository subscriptionRepository,
    ITenantRepository tenantRepository

    )
  {
    _stripeCustomerRepository = stripeCustomerRepository;
    _subscriptionRepository = subscriptionRepository;
    _tenantRepository = tenantRepository;
  }
  public async Task Handle(Subscription? stripeSubscription)
  {

    if (stripeSubscription is null)
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

    var customer = await _stripeCustomerRepository.FindByCustomerId(stripeSubscription.CustomerId, default);

    if (customer is null)
    {
      return;
    }

    var subscriptionType = stripeSubscription.Metadata.FirstOrDefault(x => x.Key == "type");

    var status =
      stripeSubscription.Status == "incomplete" ? ESubscriptionStatus.INCOMPLETE
        : stripeSubscription.Status == "incomplete_expired" ? ESubscriptionStatus.INCOMPLETE_EXPIRED
        : stripeSubscription.Status == "trialing" ? ESubscriptionStatus.TRIALING
        : stripeSubscription.Status == "active" ? ESubscriptionStatus.ACTIVE
        : stripeSubscription.Status == "past_due" ? ESubscriptionStatus.PAST_DUE
        : stripeSubscription.Status == "canceled" ? ESubscriptionStatus.CANCELED
        : stripeSubscription.Status == "unpaid" ? ESubscriptionStatus.UNPAID
        : stripeSubscription.Status == "paused" ? ESubscriptionStatus.PAUSED
          : ESubscriptionStatus.INCOMPLETE;

    if (subscriptionType.Value == ESubscriptionType.USER.ToString())
    {
      var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

      if (subscription is null)
      {
        return;
      }

      subscription.SetStatus(status);
      subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);
      await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
    }
    else if (subscriptionType.Value == ESubscriptionType.TENANT.ToString())
    {
      var tenant = await _tenantRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

      if (tenant is null)
      {
        return;
      }
      tenant.SetSubscriptionStatus(status);
      tenant.SetSubscriptionCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);
      await _tenantRepository.UpdateAsync(tenant, new CancellationToken());
    }
  }
}
