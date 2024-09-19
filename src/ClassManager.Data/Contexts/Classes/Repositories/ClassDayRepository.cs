using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

  public async Task<List<ClassDay>> ListByTenantOrClassAndDate(List<Guid>? tenantIds, List<Guid>? classesIds, DateTime date)
  {
    var zeroTime = date.Date;
    var finalTime = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

    return await DbSet
    .AsNoTracking()
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .Include((x) => x.Class)
    .ThenInclude((c) => c.Tenant)
    .Where(x => tenantIds.IsNullOrEmpty() || tenantIds.Contains(x.Class.TenantId))
    .Where(x => classesIds.IsNullOrEmpty() || classesIds.Contains(x.ClassId))
    .Where(x => x.Date >= zeroTime && x.Date <= finalTime)
    .OrderBy(x => x.Date)
    .ToListAsync();
  }

  public async Task<ClassDay> GetByIdAndTenantIdAsync(Guid tenantId, Guid id)
  {
    return await DbSet
    .AsNoTracking()
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .FirstAsync((x) => x.Class.TenantId == tenantId && x.Id == id);
  }

  public async Task<List<ClassDay>> ListByTenantId(Guid tenantId)
  {
    return await DbSet
      .Where(x => x.Class.TenantId == tenantId)
      .ToListAsync();
  }
}
