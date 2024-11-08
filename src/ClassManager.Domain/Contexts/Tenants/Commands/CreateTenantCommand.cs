using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateTenantCommand : Notifiable, ICommand
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Guid PlanId { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string State { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(FirstName, 3, "FirstName", "FirstName min 3 characters")
      .HasMaxLen(FirstName, 40, "FirstName", "FirstName max 40 characters")
      .HasMinLen(LastName, 3, "LastName", "LastName min 3 characters")
      .HasMaxLen(LastName, 40, "LastName", "LastName max 40 characters")
      .HasMinLen(Username, 3, "Username", "Username min 3 characters")
      .HasMaxLen(Username, 40, "Username", "Username max 40 characters")
      .HasMaxLen(Description, 200, "Description", "Description max 200 characters")
      .IsNotNull(PlanId, "PlanId", "PlanId cannot be null")
      .IsEmail(Email, "Email", "Invalid email")
    );
    }
  }
}