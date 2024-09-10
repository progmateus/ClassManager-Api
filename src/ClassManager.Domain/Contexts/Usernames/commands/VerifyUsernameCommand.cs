using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Tenants.Commands
{
  public class VerifyUsernameCommand : Notifiable, ICommand
  {
    public string Username { get; set; } = null!;

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .Matchs(Username, "^(?!.*\\.\\.)(?!.*\\.$)[^\\W][\\w.]{0,29}$", "Username", "Invalid username")
    );
    }
  }
}