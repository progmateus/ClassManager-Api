using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ClassRepository : Repository<Class>, IClassRepository
{
  public ClassRepository(AppDbContext context) : base(context) { }

  public async Task<Class> GetByIdAndTenantId(Guid tenantId, Guid planId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstAsync((x) => x.TenantId == tenantId && x.Id == planId, cancellationToken);
  }

  public async Task<List<Class>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where((x) => x.TenantId == tenantId).ToListAsync();
  }

  public async Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Name == name, cancellationToken);
  }

  public async Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync((x) => x.Name == name);
  }
}
