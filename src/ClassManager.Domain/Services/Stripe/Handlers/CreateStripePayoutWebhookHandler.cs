using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class CreateStripePayoutWebhookHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IPayoutRepository _payoutRepository;

  public CreateStripePayoutWebhookHandler(
    ITenantRepository tenantRepository,
    IPayoutRepository payoutRepository

    )
  {
    _tenantRepository = tenantRepository;
    _payoutRepository = payoutRepository;
  }
  public async Task Handle(Payout? stripePayout)
  {
    if (stripePayout is null)
    {
      return;
    }

    var tenant = await _tenantRepository.FindByStripeAccountId(stripePayout.Destination.AccountId, default);

    if (tenant is null)
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

    var payout = new Contexts.Invoices.Entities.Payout(stripePayout.Id, stripePayout.Amount, stripePayout.Currency, tenant.Id, status);

    await _payoutRepository.CreateAsync(payout, new CancellationToken());

  }
}
