using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class StudentsClassesRepository : Repository<StudentsClasses>, IStudentsClassesRepository
{
  public StudentsClassesRepository(AppDbContext context) : base(context) { }

  public async Task<StudentsClasses> GetByUserIdAndClassId(Guid classId, Guid userId)
  {
    return await DbSet.FirstOrDefaultAsync((tc) => tc.ClassId == classId && tc.UserId == userId);
  }

  public async Task<List<StudentsClasses>> GetByUserIdAndTenantId(Guid tenantId, Guid userId)
  {
    return await DbSet.Include(x => x.Class).Where((sc) => sc.Class.TenantId == tenantId && sc.UserId == userId).ToListAsync();
  }
}
