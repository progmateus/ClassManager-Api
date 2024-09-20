using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Entities
{
  public class ScheduleDay : TenantEntity
  {
    public ScheduleDay(Guid timeTableId, EWeekDay weekDay, string? hourStart, string? hourEnd, Guid tenantId)
    {
      TimeTableId = timeTableId;
      WeekDay = weekDay;
      HourStart = hourStart;
      HourEnd = hourEnd;
      TenantId = tenantId;
    }

    protected ScheduleDay()
    {

    }
    public Guid TimeTableId { get; private set; }
    public EWeekDay WeekDay { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public TimeTable? TimeTable { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
  }
}