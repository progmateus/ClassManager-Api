using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class GetUserClassesCommand : Notifiable, ICommand
  {
    public Guid UserId { get; set; }
    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(UserId, "UserId", "UserId cannot be null")
    );
    }
  }
}