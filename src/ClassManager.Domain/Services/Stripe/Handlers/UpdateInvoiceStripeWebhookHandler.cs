using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateInvoiceStripeWebhookHandler
{
  private readonly IInvoiceRepository _invoiceRepository;

  public UpdateInvoiceStripeWebhookHandler(
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
    var invoice = await _invoiceRepository.FindByStripeInvoiceId(stripeInvoice.Id);

    if (invoice is null)
    {
      return;
    }

    invoice.SetStripeHostedUrl(stripeInvoice.HostedInvoiceUrl);
    await _invoiceRepository.UpdateAsync(invoice, new CancellationToken());
  }
}
