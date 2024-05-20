using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class UpdateClassDayCommand : Notifiable, ICommand
  {
    public EClassDayStatus Status { get; set; }
    public string? Observation { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Observation, 3, "UpdateClassDayCommand.Observation", "Observation min 3 characters")
      .HasMaxLen(Observation, 10, "UpdateClassDayCommand.Observation", "Observation max 40 characters")
      .IsNotNull(Status, "UpdateClassDayCommand.Status", "Status not null")
    );
    }
  }
}