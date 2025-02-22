using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TeachersClassesRepository : Repository<TeachersClasses>, ITeacherClassesRepository
{
  public TeachersClassesRepository(AppDbContext context) : base(context) { }

  public int CountByClassId(Guid classId)
  {
    return DbSet.Count(x => x.ClassId == classId);
  }

  public async Task<TeachersClasses> FindByUserIdAndClassId(Guid classId, Guid userId)
  {
    return await DbSet.FirstOrDefaultAsync((tc) => tc.ClassId == classId && tc.UserId == userId);
  }

  public async Task<List<TeachersClasses>> GetByUsersIdsAndClassesIds(Guid tenantId, List<Guid>? usersIds, List<Guid>? classesIds)
  {
    return await DbSet
    .AsNoTracking()
    .Include(x => x.Class)
    .Where((tc) => (classesIds.IsNullOrEmpty() || classesIds.Contains(tc.ClassId)) && (usersIds.IsNullOrEmpty() || usersIds.Contains(tc.UserId)) && tc.Class.TenantId == tenantId)
    .ToListAsync();
  }

  public async Task<List<TeachersClasses>> GetByUsersIdsAndTenantActive(List<Guid> usersIds, CancellationToken cancellationToken = default)
  {
    return await DbSet.Where((tc) => usersIds.Contains(tc.UserId) && tc.Class.Tenant.Status == ETenantStatus.ACTIVE).ToListAsync();
  }

  public async Task<List<TeachersClasses>> ListByClassId(Guid classId, Guid tenantId)
  {
    return await DbSet.
    Include(x => x.User)
    .ThenInclude(
      u => u.Subscriptions.Where(s => s.TenantId == tenantId)
    ).Where((tc) => tc.ClassId == classId).ToListAsync();
  }

  public async Task<List<TeachersClasses>> ListByUserOrClassOrTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Include(x => x.Class)
    .Include(x => x.User)
    .Where(x => classesIds.IsNullOrEmpty() || classesIds.Contains(x.ClassId))
    .Where(x => tenantsIds.IsNullOrEmpty() || tenantsIds.Contains(x.Class.TenantId))
    .Where(x => x.Class.Tenant.Status == ETenantStatus.ACTIVE)
    .Where(x => search.IsNullOrEmpty() | x.User.Name.Contains(search) || x.User.Username.Contains(search))
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task DeleteByClassId(Guid tenantId, Guid classId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where((tc) => tc.Class.TenantId == tenantId && tc.ClassId == classId));
    await SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteByUsersAndClasses(Guid tenantId, List<Guid> classesIds, List<Guid> usersIds, CancellationToken cancellationToken)
  {
    DbSet
    .RemoveRange(DbSet
    .Where((sc) => sc.Class.TenantId == tenantId && (classesIds.IsNullOrEmpty() || classesIds.Contains(sc.ClassId) && usersIds.Contains(sc.UserId))));
    await SaveChangesAsync(cancellationToken);
  }
}
