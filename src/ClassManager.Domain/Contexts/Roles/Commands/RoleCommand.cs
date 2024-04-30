using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class RoleCommand : Notifiable, ICommand
{
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .HasMinLen(Name, 3, "RoleCommand.Name", "Name min 3 characters")
    .HasMaxLen(Name, 80, "RoleCommand.Name", "Name max 80 characters")
    .HasMinLen(Description, 3, "RoleCommand.Description", "Description min 3 characters")
    .HasMaxLen(Description, 100, "RoleCommand.Description", "Description min 80 characters")
    );
  }
}