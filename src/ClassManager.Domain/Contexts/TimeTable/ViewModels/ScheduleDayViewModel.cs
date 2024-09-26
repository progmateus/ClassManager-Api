namespace ClassManager.Domain.Contexts.TimesTables.ViewModels
{
  public class ScheduleDayViwModel
  {
    public Guid TimeTableId { get; private set; }
    public int WeekDay { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public TimeTableViewModel? TimeTable { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

  }
}