using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Auth.Commands;

public class AuthCommand : Notifiable, ICommand
{
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;

  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsEmail(Email, "AuthResponseCommand.Email", "Inválid Email")
    .IsNotNull(Password, "AuthResponseCommand.Password", "Password not be null")
  );
  }
}