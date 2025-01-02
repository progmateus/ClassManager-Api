using System.Text.RegularExpressions;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{

  public class UpdateTenantCommand : Notifiable, ICommand
  {

    /*     public UpdateTenantCommand()
        {
          Document = Regex.Replace(Document, "/W/g", "");
        } */
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<LinkCommand> Links { get; set; } = [];

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "Name", "Name min 3 characters")
      .HasMaxLen(Name, 40, "Name", "Name max 40 characters")
      .IsEmail(Email, "Email", "Invalid email")
      .HasMaxLen(Description, 200, "Description", "Description max 200 characters")
      .Matchs(Document, "([0-9]{2}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[\\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[-]?[0-9]{2})", "Document", "Invalid document")
    );
    }
  }
}