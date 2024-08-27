using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

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

  public async Task<StudentsClasses> GetByUserIdAndClassId(Guid classId, Guid userId)
  {
    return await DbSet.FirstOrDefaultAsync((tc) => tc.ClassId == classId && tc.UserId == userId);
  }

  public async Task<List<StudentsClasses>> GetByUserIdAndTenantId(Guid tenantId, Guid userId)
  {
    return await DbSet.Include(x => x.Class).Where((sc) => sc.Class.TenantId == tenantId && sc.UserId == userId).ToListAsync();
  }

  public async Task<List<StudentsClasses>> ListByClassId(Guid classId)
  {
    return await DbSet.Include(x => x.User).Where((sc) => sc.ClassId == classId).ToListAsync();
  }
}
