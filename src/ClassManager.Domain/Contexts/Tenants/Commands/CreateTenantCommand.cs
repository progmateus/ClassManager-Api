using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateTenantCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public string Document { get; set; } = null!;
    public string Number { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "CreateTenantCommand.Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "CreateTenantCommand.Name", "Name max 40 characters")
      .HasMinLen(Username, 3, "CreateTenantCommand.Username", "Username min 3 characters")
      .HasMaxLen(Username, 40, "CreateTenantCommand.Username", "Username max 40 characters")
      .HasMaxLen(Description, 200, "CreateTenantCommand.Description", "Description max 200 characters")
      .IsEmail(Email, "CreateTenantCommand.Email", "Invalid email")
    );
    }
  }
}