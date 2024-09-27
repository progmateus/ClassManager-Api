using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.TimesTables.Commands
{

  public class CreateScheduleParams
  {
    public EWeekDay WeekDay { get; set; }
    public string HourStart { get; set; } = string.Empty;
    public string HourEnd { get; set; } = string.Empty;
  }
  public class UpdateTimeTableCommand
  {
    public List<CreateScheduleParams> SchedulesDays { get; set; } = [];
  }
}