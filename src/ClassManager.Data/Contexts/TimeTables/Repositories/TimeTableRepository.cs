using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TimeTableRepository : TRepository<TimeTable>, ITimeTableRepository
{
  public TimeTableRepository(AppDbContext context) : base(context) { }

  public async Task<List<TimeTable>> FindByIdsWithInclude(List<Guid> ids, Guid tenantId)
  {
    return await DbSet
    .Include(x => x.SchedulesDays)
    .Include(x => x.Classes)
    .AsTracking()
    .Where(x => ids.Contains(x.Id) && x.TenantId == tenantId)
    .ToListAsync();
  }

  public async Task<List<TimeTable>> GetByActiveTenants()
  {
    return await DbSet
    .Include(x => x.SchedulesDays)
    .Include(x => x.Classes)
    .AsTracking()
    .Where(x => x.Tenant.Status == ETenantStatus.ACTIVE)
    .ToListAsync();
  }
}
