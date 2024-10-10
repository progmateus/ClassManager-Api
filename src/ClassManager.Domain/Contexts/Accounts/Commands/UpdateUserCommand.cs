using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class UpdateUserCommand : Notifiable, ICommand
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(FirstName, 3, "FirstName", "FirstName min 3 characters")
      .HasMaxLen(FirstName, 40, "FirstName", "FirstName max 40 characters")
      .IsEmail(Email, "Email", "Invalid Email")
      .Matchs(Phone, "\\(?([0-9]{3})\\)?([ .-]?)([0-9]{3})\\2([0-9]{4,5}|null)", "Phone", "Invalid phone")
    );
    }
  }
}