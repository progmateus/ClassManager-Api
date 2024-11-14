using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TenantPlanRepository : TRepository<TenantPlan>, ITenantPlanRepository
{
  public TenantPlanRepository(AppDbContext context) : base(context) { }

  public async Task<TenantPlan?> FindByStripePriceId(string stripePriceId, CancellationToken cancellationToken = default)
  {
    //need include to get the tenant stripe account id
    return await DbSet.Include(x => x.Tenant).FirstOrDefaultAsync((x) => x.StripePriceId == stripePriceId, cancellationToken);
  }

  public async Task<TenantPlan> GetByIdAndTenantId(Guid tenantId, Guid planId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstAsync((x) => x.TenantId == tenantId && x.Id == planId, cancellationToken);
  }

  public async Task<List<TenantPlan>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where((x) => x.TenantId == tenantId).ToListAsync();
  }

  public async Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync((x) => x.Name == name);
  }
}
