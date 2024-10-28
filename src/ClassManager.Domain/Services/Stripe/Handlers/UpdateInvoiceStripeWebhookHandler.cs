using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateInvoiceStripeWebhookHandler
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IStripeCustomerRepository _stripeCustomerRepository;
  private readonly ISubscriptionRepository _subscriptionRepository;

  public UpdateInvoiceStripeWebhookHandler(
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
    Contexts.Invoices.Entities.Invoice invoice;

    /* var invoiceEntity = await _invoiceRepository.FindByStripeInvoiceId(stripeInvoice.Id);

    if (invoiceEntity is null)
    {
      return;
    } */


    if (stripeInvoice.Status == "draft")
    {
      Console.WriteLine("É DRAFT");
      return;
    }

    var customer = await _stripeCustomerRepository.FindByCustomerId(stripeInvoice.CustomerId, default);

    if (customer is null)
    {
      Console.WriteLine("NÃO ACHOU CUSTOMER");
      return;
    }

    if (stripeInvoice.BillingReason == "subscription_create")
    {
      if (customer.Type == EStripeCustomerType.USER)
      {
        Console.WriteLine("USER");
        var subscription = await _subscriptionRepository.FindByStripeSubscriptionId(stripeInvoice.SubscriptionId, new CancellationToken());
        invoice = new Contexts.Invoices.Entities.Invoice(customer.UserId, subscription.TenantPlan.Id, subscription.Id, null, customer.TenantId, subscription.TenantPlan.Price, EInvoiceTargetType.USER, EInvoiceType.USER_SUBSCRIPTION, stripeInvoice.Id, stripeInvoice.HostedInvoiceUrl, stripeInvoice.Number);
        await _invoiceRepository.CreateAsync(invoice, new CancellationToken());
      }
      else
      {
        Console.WriteLine("TENANT");
        invoice = new Contexts.Invoices.Entities.Invoice(customer.UserId, null, null, customer.Tenant.Plan.Id, customer.TenantId, customer.Tenant.Plan.Price, EInvoiceTargetType.TENANT, EInvoiceType.TENANT_SUBSCRIPTION, stripeInvoice.Id, stripeInvoice.HostedInvoiceUrl, stripeInvoice.Number);
        await _invoiceRepository.CreateAsync(invoice, new CancellationToken());
      }
    }
  }
}
