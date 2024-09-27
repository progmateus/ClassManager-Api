using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
public interface IScheduleDayRepository : ITRepository<ScheduleDay>
{
  Task<object> GroupByWeekDay(Guid timeTableId);
  Task DeleteAllByTimeTableId(Guid timeTableId, CancellationToken cancellationToken);
}