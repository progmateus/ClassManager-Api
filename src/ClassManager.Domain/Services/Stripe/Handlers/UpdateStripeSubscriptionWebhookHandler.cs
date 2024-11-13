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

    if (customer is null || customer.Type != EStripeCustomerType.USER)
    {
      return;
    }

    var subscriptionType = stripeSubscription.Metadata.FirstOrDefault(x => x.Value == "type");

    var status =
      stripeSubscription.Status == "paused" ? ESubscriptionStatus.PAUSED
        : stripeSubscription.Status == "canceled" ? ESubscriptionStatus.CANCELED
          : ESubscriptionStatus.ACTIVE;

    if (subscriptionType.Value == "user")
    {
      var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

      if (subscription is null)
      {
        return;
      }

      subscription.ChangeStatus(status);
      await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
    }
    else if (subscriptionType.Value == "tenant")
    {
      var tenant = await _tenantRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

      if (tenant is null)
      {
        return;
      }
      tenant.UpdateSubscriptionStatus(status);
      await _tenantRepository.UpdateAsync(tenant, new CancellationToken());
    }
  }
}
