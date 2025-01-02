using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeSubscriptionWebhookHandler
{
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly ITenantRepository _tenantRepository;
  private readonly IInvoiceRepository _invoiceRepository;

  public UpdateStripeSubscriptionWebhookHandler(
    ISubscriptionRepository subscriptionRepository,
    ITenantRepository tenantRepository,
    IInvoiceRepository invoiceRepository

    )
  {
    _subscriptionRepository = subscriptionRepository;
    _tenantRepository = tenantRepository;
    _invoiceRepository = invoiceRepository;
  }
  public async Task Handle(Subscription? stripeSubscription)
  {

    if (stripeSubscription is null)
    {
      return;
    }

    var enabledStatus = new List<string> {
      "incomplete",
      "incomplete_expired",
      "active",
      "past_due",
      "canceled",
      "unpaid",
      "paused",
      };

    if (!enabledStatus.Contains(stripeSubscription.Status))
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

    if (subscriptionType.Value == ETargetType.USER.ToString())
    {
      var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

      if (subscription is null)
      {
        return;
      }

      if (subscription.LatestInvoice is not null && subscription.LatestInvoice.StripeInvoiceId != stripeSubscription.LatestInvoiceId)
      {
        var latestInvoice = await _invoiceRepository.FindByStripeInvoiceId(stripeSubscription.LatestInvoiceId);
        if (latestInvoice is not null)
        {
          subscription.SetLatestInvoice(latestInvoice.Id);
        }
      }
      subscription.SetCanceledAt(stripeSubscription.CanceledAt);
      subscription.SetStatus(status);
      subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);
      await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
    }
    else if (subscriptionType.Value == ETargetType.TENANT.ToString())
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
