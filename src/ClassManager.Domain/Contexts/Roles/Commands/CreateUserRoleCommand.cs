using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class CreateUserRoleCommand : Notifiable, ICommand
{
  public Guid UserId { get; set; }
  public string RoleName { get; set; } = null!;
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsNotNull(UserId, "UsersRolesCommand.UserId", "UserId not null")
    .IsNotNull(RoleName, "UsersRolesCommand.RoleName", "RoleName not null")
    );
  }
}