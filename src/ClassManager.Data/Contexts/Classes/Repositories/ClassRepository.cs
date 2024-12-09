using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ClassRepository : TRepository<Class>, IClassRepository
{
  public ClassRepository(AppDbContext context) : base(context) { }

  public async Task<Class?> FindByIdWithTimeTable(Guid id, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Include((x) => x.TimeTable)
    .ThenInclude(tt => tt.SchedulesDays)
    .FirstOrDefaultAsync((x) => x.Id == id, cancellationToken);
  }

  public async Task<Class?> FindClassProfile(Guid tenantId, Guid classId, CancellationToken cancellationToken)
  {
    return await DbSet
    .Include(x => x.Address)
    .Include(x => x.TeachersClasses)
    .ThenInclude(x => x.User)
    .FirstOrDefaultAsync((x) => x.TenantId == tenantId && x.Id == classId, cancellationToken);
  }

  public async Task<Class?> GetByIdAndTenantIdAsync(Guid tenantId, Guid classId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync((x) => x.TenantId == tenantId && x.Id == classId, cancellationToken);
  }

  public async Task<List<Class>> GetByTenantsIds(List<Guid> tenantsIds, CancellationToken cancellationToken = default)
  {
    return await DbSet.Where((x) => tenantsIds.Contains(x.TenantId)).ToListAsync(cancellationToken);
  }

  public async Task<List<Class>> ListByTenantId(Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.Where((x) => x.TenantId == tenantId).ToListAsync(cancellationToken);
  }

  public async Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Name == name, cancellationToken);
  }

  public async Task<bool> PlanAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AnyAsync((x) => x.Name == name);
  }
  public async Task<List<ClassPreviewViewModel>> ListPreviewsWithPagination(Guid tenantId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Where(x => x.TenantId == tenantId)
    .Select(x => new ClassPreviewViewModel
    {
      Id = x.Id,
      Description = x.Description,
      Name = x.Name,
      StudentsCount = x.StudentsClasses.Count(),
      TeachersCount = x.TeachersClasses.Count(),
      TenantId = x.TenantId,
      CreatedAt = x.CreatedAt,
      UpdatedAt = x.UpdatedAt,
    })
    .Skip(skip)
    .Take(limit)
    .ToListAsync(cancellationToken);
  }
}
