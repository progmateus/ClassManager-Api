using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class CreateTimeTableCommand : Notifiable, ICommand
{
  public string Name { get; set; } = string.Empty!;
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .HasMinLen(Name, 3, "CreateTimeTableCommand.FirstName", "FirstName min 3 characters")
    .HasMaxLen(Name, 40, "CreateTimeTableCommand.FirstName", "FirstName max 40 characters")
    );
  }
}