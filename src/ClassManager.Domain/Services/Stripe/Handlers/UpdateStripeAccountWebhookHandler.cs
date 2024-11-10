using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeAccountWebhookHandler
{
  private readonly ITenantRepository _tenantRepository;

  public UpdateStripeAccountWebhookHandler(
    ITenantRepository tenantRepository

    )
  {
    _tenantRepository = tenantRepository;
  }
  public async Task Handle(Account? stripeAccount)
  {
    if (stripeAccount is null)
    {
      return;
    }


    var tenant = await _tenantRepository.FindByStripeAccountId(stripeAccount.Id, new CancellationToken());

    if (tenant is null)
    {
      return;
    }
    tenant.UpdateChargesEnabled(stripeAccount.ChargesEnabled);
    await _tenantRepository.UpdateAsync(tenant, new CancellationToken());
  }
}
