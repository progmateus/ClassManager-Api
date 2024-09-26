using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ScheduleDayRepository : TRepository<ScheduleDay>, IScheduleDayRepository
{
  public ScheduleDayRepository(AppDbContext context) : base(context) { }

  public async Task<object> GroupByWeekDay(Guid timeTableId)
  {
    return await DbSet
    .Where(x => x.TimeTableId == timeTableId)
    .GroupBy(x => x.WeekDay)
    .Select(g => new
    {
      Weekday = g.Key,
      Times = g.OrderBy(x => x.HourStart).Select(x => x)
    })
    .ToListAsync();
  }
}


/* public async Task<object> ListByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)
  {

    return await DbSet
    .Include(x => x.User)
    .Include(x => x.TenantPlan)
    .Where(x => x.TenantId == tenantId)
    .GroupBy(x => x.UserId)
    .Select(x => new
    {
      Subscription = x.OrderByDescending(x => x.CreatedAt).Select(x => x).First()
    }).ToListAsync(cancellationToken);
  } */
