namespace ClassManager.Domain.Contexts.TimesTables.ViewModels
{
  public class ScheduleDayViewModel
  {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid TimeTableId { get; set; }
    public int WeekDay { get; set; }
    public string? HourStart { get; set; }
    public string? HourEnd { get; set; }
    public TimeTableViewModel? TimeTable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

  }
}