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
      .HasMaxLen(Observation, 40, "Observation", "Observation max 40 characters")
      .IsNotNull(Status, "Status", "Status not null")
    );
    }
  }
}