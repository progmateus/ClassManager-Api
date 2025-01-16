using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
  public PlanRepository(AppDbContext context) : base(context) { }

  public async Task<Plan?> FindByStripePriceId(string stripePriceId, CancellationToken cancellationToken = default)
  {
    return await DbSet.FirstOrDefaultAsync((x) => x.StripePriceId == stripePriceId, cancellationToken);
  }
}
