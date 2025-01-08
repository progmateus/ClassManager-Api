using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ExternalBankAccountRepository : TRepository<ExternalBankAccount>, IExternalBankAccountRepository
{
  public ExternalBankAccountRepository(AppDbContext context) : base(context) { }

  public async Task<ExternalBankAccount?> FindByStripeExternalBankAccountId(string stripeExternalBanAccountId, CancellationToken cancellationToken = new CancellationToken())
  {
    return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.StripeExternalBankAccountId == stripeExternalBanAccountId, cancellationToken);
  }
}
