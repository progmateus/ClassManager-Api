using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TimeTableRepository : TRepository<TimeTable>, ITimeTableRepository
{
  public TimeTableRepository(AppDbContext context) : base(context) { }

  public async Task<TimeTable?> FindByIdAndTenantIdWithGroupBy(Guid tenantId, Guid timeTableId)
  {
    return await DbSet
    .Include(x => x.SchedulesDays.GroupBy(x => x.TimeTableId))
    .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == timeTableId);
  }
}
