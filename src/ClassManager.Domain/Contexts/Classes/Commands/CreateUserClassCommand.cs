using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class CreateUserClassCommand : Notifiable, ICommand
  {
    public Guid UserId { get; set; }
    public Guid ClassId { get; set; }
    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(UserId, "CreateUserClassCommand.UserId", "UserId not null")
      .IsNotNull(ClassId, "CreateUserClassCommand.ClassId", "ClassId not null")
    );
    }
  }
}