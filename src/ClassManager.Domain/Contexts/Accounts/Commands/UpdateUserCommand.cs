using System.Text.RegularExpressions;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class UpdateUserCommand : Notifiable, ICommand
  {

    /*     public UpdateUserCommand()
        {
          Document = Regex.Replace(Document, "/W/g", "");
        } */
    public string Name { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(Name, 3, "Name", "Name min 3 characters")
      .HasMaxLen(Name, 150, "Name", "Name max 150 characters")
      .IsEmail(Email, "Email", "Invalid Email")
      .Matchs(Phone, "(\\(?\\d{2}\\)?) ?(9{1})? ?(\\d{4})-? ?(\\d{4})", "Phone", "Invalid phone")
      .Matchs(Document, "(^[0-9]{3}.?[0-9]{3}.?[0-9]{3}-?[0-9]{2}$)", "Document", "Invalid document")
    );
    }
  }
}