using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
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

  public async Task<List<ClassDay>> ListByTenantOrClassAndDate(List<Guid> tenantIds, List<Guid> classesIds, DateTime date, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    var zeroTime = date.Date;
    var finalTime = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

    return await DbSet
    .AsNoTracking()
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .Include((x) => x.Class)
    .ThenInclude((c) => c.Tenant)
    .Where(x => tenantIds.Contains(x.Class.TenantId) || classesIds.Contains(x.ClassId))
    .Where(x => x.Date >= zeroTime && x.Date <= finalTime)
    .OrderBy(x => x.Date)
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task<List<ClassDay>> ListByTenantId(Guid tenantId)
  {
    return await DbSet
      .Where(x => x.Class.TenantId == tenantId)
      .ToListAsync();
  }

  public async Task<ClassDay?> FindClassDayProfile(Guid tenantId, Guid classDayId)
  {
    return await DbSet
    .AsNoTracking()
    .Include(x => x.Class)
    .ThenInclude(x => x.TeachersClasses)
    .ThenInclude(tc => tc.User)
    .Include((x) => x.Bookings)
    .ThenInclude((b) => b.User)
    .FirstOrDefaultAsync((x) => x.Class.TenantId == tenantId && x.Id == classDayId);
  }

  public async Task DeleteAllAfterAndBeforeDate(List<Guid> classesIds, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken)
  {
    DbSet.RemoveRange(DbSet.Where((cd) => classesIds.Contains(cd.ClassId) && cd.Date > initialDate && cd.Date < finalDate));
    await SaveChangesAsync(cancellationToken);
  }
}
