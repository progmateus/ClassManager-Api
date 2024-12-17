using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class UpdateUserCommand : Notifiable, ICommand
  {
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
    );
    }
  }
}