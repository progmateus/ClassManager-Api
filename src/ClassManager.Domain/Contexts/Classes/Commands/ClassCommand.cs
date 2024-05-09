using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class ClassCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = null!;
    public string BusinessHour { get; set; } = null!;
    public string Description { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "ClassCommand.Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "ClassCommand.Name", "Name max 40 characters")
      .HasMaxLen(Description, 200, "ClassCommand.Description", "Description max 40 characters")
    );
    }
  }
}