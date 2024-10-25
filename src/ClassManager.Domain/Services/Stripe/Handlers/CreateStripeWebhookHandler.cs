using ClassManager.Domain.Services.Stripe.Repositories.Contracts;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class CreateStripeWebhookHandler
{
  private readonly IPaymentService _paymentService;

  public CreateStripeWebhookHandler(
    IPaymentService paymentService

    )
  {
    _paymentService = paymentService;
  }
  public void Handle()
  {
    _paymentService.CreateWebhook();
  }
}
