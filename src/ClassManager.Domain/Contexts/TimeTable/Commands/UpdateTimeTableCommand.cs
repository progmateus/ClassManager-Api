using ClassManager.Domain.Contexts.Shared.Enums;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.TimesTables.Commands
{

  public class CreateScheduleParams
  {
    public string Name { get; set; } = string.Empty;
    public EWeekDay WeekDay { get; set; }
    public string HourStart { get; set; } = string.Empty;
    public string HourEnd { get; set; } = string.Empty;

  }
  public class UpdateTimeTableCommand : Notifiable
  {
    public string Name { get; set; } = string.Empty;
    public List<CreateScheduleParams> SchedulesDays { get; set; } = [];

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "Name", "Name min 3 characters")
      .HasMaxLen(Name, 150, "Name", "Name max 150 characters")
    );
    }
  }

}