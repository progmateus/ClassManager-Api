using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripePayoutWebhookHandler
{
  private readonly IPayoutRepository _payoutRepository;

  public UpdateStripePayoutWebhookHandler(
    IPayoutRepository payoutRepository

    )
  {
    _payoutRepository = payoutRepository;
  }
  public async Task Handle(Payout? stripePayout)
  {
    if (stripePayout is null)
    {
      return;
    }

    var payout = await _payoutRepository.FindByStripePayoutId(stripePayout.Id);

    if (payout is null)
    {
      return;
    }

    var status =
      stripePayout.Status == "paid" ? EPayoutStatus.PAID
        : stripePayout.Status == "pending" ? EPayoutStatus.PENDING
          : stripePayout.Status == "in_transit" ? EPayoutStatus.IN_TRANSIT
            : stripePayout.Status == "canceled" ? EPayoutStatus.CANCELED
              : stripePayout.Status == "failed" ? EPayoutStatus.FAILED
                : EPayoutStatus.FAILED;

    payout.SetStatus(status);

    await _payoutRepository.UpdateAsync(payout, new CancellationToken());
  }
}
