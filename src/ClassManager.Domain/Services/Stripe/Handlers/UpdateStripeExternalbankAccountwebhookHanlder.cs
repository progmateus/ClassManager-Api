using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Stripe;

namespace ClassManager.Domain.Services.Stripe.Handlers;

public class UpdateStripeExternalBankAccountWebhookHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IExternalBankAccountRepository _externalBankAccountRepository;

  public UpdateStripeExternalBankAccountWebhookHandler(
    ITenantRepository tenantRepository,
    IExternalBankAccountRepository externalBankAccountRepository

    )
  {
    _tenantRepository = tenantRepository;
    _externalBankAccountRepository = externalBankAccountRepository;
  }
  public async Task Handle(BankAccount? stripeBankAccount)
  {
    if (stripeBankAccount is null)
    {
      return;
    }

    var externalBankAccount = await _externalBankAccountRepository.FindByStripeExternalBankAccountId(stripeBankAccount.Id, new CancellationToken());

    if (externalBankAccount is null)
    {
      return;
    }

    var status =
      stripeBankAccount.Status == "new" ? EExternalBankAccountStatus.NEW
        : stripeBankAccount.Status == "verifies" ? EExternalBankAccountStatus.VERIFIED
          : stripeBankAccount.Status == "verification_failed" ? EExternalBankAccountStatus.VERIFICATION_FAILED
            : stripeBankAccount.Status == "validated" ? EExternalBankAccountStatus.VALIDATED
              : stripeBankAccount.Status == "errores" ? EExternalBankAccountStatus.ERRORED
                : EExternalBankAccountStatus.ERRORED;

    externalBankAccount.Update(
      stripeBankAccount.BankName,
      stripeBankAccount.Country,
      stripeBankAccount.Currency,
      stripeBankAccount.Last4,
      stripeBankAccount.RoutingNumber,
      status
    );

    await _externalBankAccountRepository.UpdateAsync(externalBankAccount, new CancellationToken());

  }
}
