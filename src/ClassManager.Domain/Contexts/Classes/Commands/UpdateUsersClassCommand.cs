using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Classes.Commands
{
  public class UpdateUsersClassCommand : Notifiable, ICommand
  {
    public List<Guid> UsersIds { get; set; } = [];
    public Guid ClassId { get; set; }
    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(ClassId, "ClassId", "ClassId cannot be null")
    );
    }
  }
}