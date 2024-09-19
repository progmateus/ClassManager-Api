using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.ClassDays.Commands
{

  public class CreateClassHourParams
  {
    public EWeekDay WeekDay { get; set; }
    public string HourStart { get; set; } = string.Empty;
    public string HourEnd { get; set; } = string.Empty;
  }
  public class CreateClassHourCommand
  {
    public List<CreateClassHourParams> ClassesHours { get; set; } = [];
  }
}