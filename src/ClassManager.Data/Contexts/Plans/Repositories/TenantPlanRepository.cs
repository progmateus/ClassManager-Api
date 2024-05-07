using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TenantPlanRepository : Repository<TenantPlan>, ITenantPlanRepository
{
  public TenantPlanRepository(AppDbContext context) : base(context) { }

  public async Task<TenantPlan> GetByIdAndTenantId(Guid planId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstAsync((x) => x.TenantId == tenantId && x.Id == planId, cancellationToken);
  }

  public List<TenantPlan> ListByTenantId(Guid tenantId, CancellationToken cancellationToken)
  {
    return DbSet.Where((x) => x.TenantId == tenantId).ToList();
  }

  public async Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync((x) => x.Name == name);
  }
}
