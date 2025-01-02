using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class PayoutRepository : TRepository<Payout>, IPayoutRepository
{
  public PayoutRepository(AppDbContext context) : base(context) { }

  public async Task<Payout?> FindByStripePayoutId(string stripePayoutId)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.StripePayoutId == stripePayoutId);
  }
}
