using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class FinalizeStripeInvoiceWebhookHandler
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;

  public FinalizeStripeInvoiceWebhookHandler(
    IInvoiceRepository invoiceRepository,
    IStripeCustomerRepository stripeCustomerRepository,
    ISubscriptionRepository subscriptionRepository

    )
  {
    _invoiceRepository = invoiceRepository;
    _stripeCustomerRepository = stripeCustomerRepository;
    _subscriptionRepository = subscriptionRepository;
  }
  public async Task Handle(Invoice? stripeInvoice)
  {
    if (stripeInvoice is null)
    {
      return;
    }

    if (stripeInvoice.Status == "draft")
    {
      return;
    }

    var customer = await _stripeCustomerRepository.FindByCustomerId(stripeInvoice.CustomerId, default);

    if (customer is null)
    {
      return;
    }

    var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeInvoice.SubscriptionId, new CancellationToken());

    if (subscription is null)
    {
      return;
    }


    var invoice = new Contexts.Invoices.Entities.Invoice(customer.UserId, subscription.TenantPlanId, subscription.Id, subscription.PlanId, customer.TenantId, subscription.TenantPlan.Price, subscription.TargetType, EInvoiceType.USER_SUBSCRIPTION, stripeInvoice.Id, stripeInvoice.HostedInvoiceUrl, stripeInvoice.Number);

    await _invoiceRepository.CreateAsync(invoice, new CancellationToken());

    if (stripeInvoice.BillingReason == "subscription_cycle")
    {
      subscription.SetLatestInvoice(invoice.Id);
      await _subscriptionRepository.UpdateAsync(subscription, new CancellationToken());
    }
  }
}
