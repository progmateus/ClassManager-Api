using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class UserClassCommand : Notifiable, ICommand
  {
    public Guid UserId { get; set; }
    public Guid ClassId { get; set; }
    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(UserId, "UserId", "UserId cannot be null")
      .IsNotNull(ClassId, "ClassId", "ClassId cannot be null")
    );
    }
  }
}