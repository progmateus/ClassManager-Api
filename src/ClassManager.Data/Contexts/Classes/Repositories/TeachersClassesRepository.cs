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

  public async Task<TeachersClasses> GetByUserIdAndClassId(Guid classId, Guid userId)
  {
    return await DbSet.FirstOrDefaultAsync((tc) => tc.ClassId == classId && tc.UserId == userId);
  }

  public async Task<List<TeachersClasses>> GetByUserIdAndTenantId(Guid tenantId, Guid userId)
  {
    return await DbSet.Include(x => x.Class).Where((tc) => tc.Class.TenantId == tenantId && tc.UserId == userId).ToListAsync();
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
    .Where(x => (usersIds.IsNullOrEmpty() || usersIds.Contains(x.UserId)) && (classesIds.IsNullOrEmpty() || classesIds.Contains(x.ClassId)) || (tenantsIds.IsNullOrEmpty() || tenantsIds.Contains(x.Class.TenantId)) && x.Class.Tenant.Status == ETenantStatus.ACTIVE)
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }
}
