using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

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

  public async Task<List<TeachersClasses>> ListByClassId(Guid classId, Guid tenantId)
  {
    return await DbSet.
    Include(x => x.User)
    .ThenInclude(
      u => u.Subscriptions.Where(s => s.TenantId == tenantId)
    ).Where((tc) => tc.ClassId == classId).ToListAsync();
  }



}
