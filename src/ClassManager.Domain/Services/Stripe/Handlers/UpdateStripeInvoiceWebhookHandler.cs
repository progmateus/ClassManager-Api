using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeInvoiceWebhookHandler
{
  private readonly IInvoiceRepository _invoiceRepository;

  public UpdateStripeInvoiceWebhookHandler(
    IInvoiceRepository invoiceRepository
    )
  {
    _invoiceRepository = invoiceRepository;
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

    var invoiceEntity = await _invoiceRepository.FindByStripeInvoiceId(stripeInvoice.Id);

    if (invoiceEntity is null)
    {
      return;
    }

    var status =
      stripeInvoice.Status == "open" ? EInvoiceStatus.OPEN
        : stripeInvoice.Status == "paid" ? EInvoiceStatus.PAID
          : stripeInvoice.Status == "void" ? EInvoiceStatus.VOID
            : EInvoiceStatus.UNCOLLECTIBLE;

    invoiceEntity.UpdateStatus(status);

    await _invoiceRepository.UpdateAsync(invoiceEntity, new CancellationToken());
  }
}
