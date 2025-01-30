using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Auth.Commands;

public class RefreshTokenCommand : Notifiable, ICommand
{
  public string RefreshToken { get; set; } = string.Empty;

  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsNotNull(RefreshToken, "RefreshToken", "RefreshToken not be null")
  );
  }
}