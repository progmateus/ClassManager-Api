using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Repositories.Contracts
{
  public interface IExternalBankAccountRepository : ITRepository<ExternalBankAccount>
  {
    Task<ExternalBankAccount?> FindByStripeExternalBankAccountId(string stripeExternalBanAccountId, CancellationToken cancellationToken);
  }
}
