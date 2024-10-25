using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateTenantCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public string Document { get; set; } = null!;
    public string Phone { get; set; } = string.Empty;
    public Guid PlanId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "Name", "Name max 40 characters")
      .HasMinLen(Username, 3, "Username", "Username min 3 characters")
      .HasMaxLen(Username, 40, "Username", "Username max 40 characters")
      .HasMaxLen(Description, 200, "Description", "Description max 200 characters")
      .IsNotNull(PlanId, "PlanId", "PlanId cannot be null")
      .IsEmail(Email, "Email", "Invalid email")
    );
    }
  }
}