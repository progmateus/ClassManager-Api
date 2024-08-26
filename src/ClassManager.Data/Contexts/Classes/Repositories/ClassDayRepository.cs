using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ClassDayRepository : Repository<ClassDay>, IClassDayRepository
{
  public ClassDayRepository(AppDbContext context) : base(context) { }

  public object CountByClassId(Guid classId, DateTime initiDate, DateTime endDate)
  {
    return DbSet
              .Where(x => x.ClassId == classId && x.Date >= initiDate && x.Date <= endDate)
              .GroupBy(x => x.Status)
              .Select(g => new { status = g.Key, count = g.Count() });
  }

  public async Task<ClassDay> GetByIdAndTenantIdAsync(Guid tenantId, Guid id)
  {
    return await DbSet
    .AsNoTracking()
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .FirstAsync((x) => x.Class.TenantId == tenantId && x.Id == id);
  }
}
