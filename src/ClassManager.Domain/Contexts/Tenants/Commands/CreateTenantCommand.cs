using System.Text.RegularExpressions;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class CreateTenantCommand : Notifiable, ICommand
  {

    /*     public CreateTenantCommand()
        {
          Document = Regex.Replace(Document, "/W/g", "");
        } */

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Phone { get; set; } = null!;
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
      .Matchs(Document, "([0-9]{2}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[\\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[-]?[0-9]{2})", "Document", "Invalid document")
    );
    }
  }
}