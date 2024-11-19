using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Data.Migrations;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class StudentsClassesRepository : Repository<StudentsClasses>, IStudentsClassesRepository
{
  public StudentsClassesRepository(AppDbContext context) : base(context) { }

  public async Task DeleteByUserIdAndtenantId(Guid tenantId, Guid userId, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where((sc) => sc.Class.TenantId == tenantId && sc.UserId == userId));
    await SaveChangesAsync(cancellationToken);
  }

  public int CountByClassId(Guid classId)
  {
    return DbSet.Count(x => x.ClassId == classId);
  }

  public async Task<StudentsClasses?> FindByUserIdAndClassId(Guid classId, Guid userId)
  {
    return await
    DbSet
    .FirstOrDefaultAsync((tc) => tc.ClassId == classId && tc.UserId == userId);
  }
  public async Task<List<StudentsClasses>> ListByClassId(Guid classId, Guid tenantId)
  {
    return await DbSet
    .Include(x => x.User)
    .ThenInclude(
      u => u.Subscriptions.Where(s => s.TenantId == tenantId)
    )
    .Where((sc) => sc.ClassId == classId).ToListAsync();
  }

  public async Task<List<StudentsClasses>> GetByUsersIdsAndTenantActive(List<Guid> usersIds, CancellationToken cancellationToken = default)
  {
    return await DbSet.Where((sc) => usersIds.Contains(sc.UserId) && sc.Class.Tenant.Status == ETenantStatus.ACTIVE).ToListAsync();
  }

  public async Task<List<StudentsClasses>> ListByUserOrClassOrTenantAsync(List<Guid> usersIds, List<Guid> tenantsIds, List<Guid> classesIds, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Include(x => x.Class)
    .Include(x => x.User)
    .Where(x => (usersIds.Contains(x.UserId) || tenantsIds.Contains(x.Class.TenantId) || classesIds.Contains(x.ClassId)) && x.Class.Tenant.Status == ETenantStatus.ACTIVE)
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task<List<StudentsClasses>> ListByUserId(Guid userId)
  {
    return await DbSet
    .Where(x => x.UserId == userId)
    .ToListAsync();
  }
}
