using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class UpdateClassTimeTableCommand : Notifiable, ICommand
  {
    public Guid TimeTableId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(TimeTableId, "TimeTableId", "TimeTableId can't be null")
    );
    }
  }
}