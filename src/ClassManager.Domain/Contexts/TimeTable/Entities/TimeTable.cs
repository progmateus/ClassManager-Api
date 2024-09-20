using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Entities
{
  public class TimeTable : TenantEntity
  {
    public TimeTable(EWeekDay weekDay, string? hourStart, string? hourEnd, Guid tenantId)
    {
      WeekDay = weekDay;
      HourStart = hourStart;
      HourEnd = hourEnd;
      TenantId = tenantId;
    }

    protected TimeTable()
    {

    }

    public EWeekDay WeekDay { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
  }
}