using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class UsersRolesCommand : Notifiable, ICommand
{
  public Guid UserId { get; set; }
  public List<Guid> RolesIds { get; set; } = new();
  public Guid TenantId { get; set; }
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsNotNull(UserId, "UsersRolesCommand.UserId", "UserId not null")
    .IsNotNull(RolesIds, "UsersRolesCommand.RoleId", "RoleId not null")
    .IsNotNull(TenantId, "UsersRolesCommand.TenantId", "TenantId not null")
    );
  }
}