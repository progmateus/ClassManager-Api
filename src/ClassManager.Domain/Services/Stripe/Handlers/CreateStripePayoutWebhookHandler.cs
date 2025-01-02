using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
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

    var payout = new Contexts.Invoices.Entities.Payout(stripePayout.Id, stripePayout.Amount, stripePayout.Currency, tenant.Id);

    await _payoutRepository.CreateAsync(payout, new CancellationToken());

  }
}
