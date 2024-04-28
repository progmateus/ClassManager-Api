using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class CreateUserCommand : Notifiable, ICommand
  {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Avatar { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(FirstName, 3, "CreateUserCommand.FirstName", "FirstName min 3 characters")
      .HasMaxLen(FirstName, 40, "CreateUserCommand.FirstName", "FirstName max 40 characters")
    );
    }
  }
}