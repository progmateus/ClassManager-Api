using System.Text.RegularExpressions;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class CreateUserCommand : Notifiable, ICommand
  {

    public CreateUserCommand()
    {
      Document = Regex.Replace(Document, "/W/g", "");
    }

    public string Name { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Avatar { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "Name", "Name min 3 characters")
      .HasMaxLen(Name, 150, "Name", "Name max 150 characters")
      .Matchs(Username, "^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,29}$", "Username", "Invalid username")
      .Matchs(Phone, "^[0-9]{10,}$", "Phone", "Invalid phone")
      .Matchs(Document, "(^d{3}.?d{3}.?d{3}-? d{2}$)", "Document", "Invalid document")
    );
    }
  }
}