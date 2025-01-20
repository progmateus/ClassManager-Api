using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeSubscriptionWebhookHandler
{
  private readonly ISubscriptionRepository _subscriptionRepository;
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly ITenantPlanRepository _tenantPlanRepository;
  private readonly IPlanRepository _planRepository;

  public UpdateStripeSubscriptionWebhookHandler(
    ISubscriptionRepository subscriptionRepository,
    IInvoiceRepository invoiceRepository,
    ITenantPlanRepository tenantPlanRepository,
    IPlanRepository planRepository

    )
  {
    _subscriptionRepository = subscriptionRepository;
    _invoiceRepository = invoiceRepository;
    _tenantPlanRepository = tenantPlanRepository;
    _planRepository = planRepository;
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

    var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeSubscription.Id, new CancellationToken());

    if (subscription is null)
    {
      return;
    }

    if (subscription.LatestInvoice is null)
    {
      var latestInvoice = await _invoiceRepository.FindByStripeInvoiceId(stripeSubscription.LatestInvoiceId);
      if (latestInvoice is not null)
      {
        subscription.SetStripeScheduleSubscriptionNextPlanId(null);
        subscription.SetLatestInvoice(latestInvoice.Id);
      }
    }
    else
    {
      if (subscription.LatestInvoice.StripeInvoiceId != stripeSubscription.LatestInvoiceId)
      {
        var latestInvoice = await _invoiceRepository.FindByStripeInvoiceId(stripeSubscription.LatestInvoiceId);
        if (latestInvoice is not null)
        {
          subscription.SetStripeScheduleSubscriptionNextPlanId(null);
          subscription.SetLatestInvoice(latestInvoice.Id);
        }
      }
    }



    var stripeSubscriptionItem = stripeSubscription.Items.Data.FirstOrDefault((x) => x.Object == "subscription_item");

    if (stripeSubscriptionItem is not null)
    {
      subscription.SetStripeSubscriptionPriceItemId(stripeSubscriptionItem.Id);

      if (subscription.TargetType == ETargetType.USER)
      {
        if (subscription.TenantPlan.StripePriceId != stripeSubscriptionItem.Price.Id)
        {

          var tenantPlan = await _tenantPlanRepository.FindByStripePriceId(stripeSubscriptionItem.Price.Id, new CancellationToken());

          if (tenantPlan is not null)
          {
            subscription.SetTenantPlan(tenantPlan.Id);
          }
        }
      }
      else
      {
        if (subscription.Plan.StripePriceId != stripeSubscriptionItem.Price.Id)
        {
          var plan = await _planRepository.FindByStripePriceId(stripeSubscriptionItem.Price.Id, new CancellationToken());

          if (plan is not null)
          {
            subscription.SetPlan(plan.Id);
          }
        }
      }

    }

    subscription.SetCanceledAt(stripeSubscription.CanceledAt);
    subscription.SetStatus(status);
    subscription.SetCurrentPeriod(stripeSubscription.CurrentPeriodStart, stripeSubscription.CurrentPeriodEnd);
    await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
  }
}
