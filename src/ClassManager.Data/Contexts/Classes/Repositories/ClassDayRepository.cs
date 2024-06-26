using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ClassDayRepository : Repository<ClassDay>, IClassDayRepository
{
  public ClassDayRepository(AppDbContext context) : base(context) { }

  public async Task<ClassDay> GetByTenantIdAndClassId(Guid tenantId, Guid classId)
  {
    return await DbSet.FirstAsync((x) => x.Class.TenantId == tenantId && x.ClassId == classId);
  }
}
