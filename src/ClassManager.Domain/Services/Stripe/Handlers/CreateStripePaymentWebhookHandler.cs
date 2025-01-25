using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class CreateStripePaymentWebhookHandler
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IPaymentRepository _paymentRepository;

  public CreateStripePaymentWebhookHandler(
    IInvoiceRepository invoiceRepository,
    IPaymentRepository paymentRepository
    )
  {
    _invoiceRepository = invoiceRepository;
    _paymentRepository = paymentRepository;
  }
  public async Task Handle(Invoice? stripeInvoice)
  {

    if (stripeInvoice is null)
    {
      return;
    }

    var invoiceEntity = await _invoiceRepository.FindByStripeInvoiceId(stripeInvoice.Id);

    if (invoiceEntity is null)
    {
      return;
    }

    var payment = new Contexts.Invoices.Entities.Payment(stripeInvoice.Total, stripeInvoice.Currency, invoiceEntity.TenantId, invoiceEntity.Id, invoiceEntity.TargetType);

    await _paymentRepository.CreateAsync(payment, new CancellationToken());
  }
}
