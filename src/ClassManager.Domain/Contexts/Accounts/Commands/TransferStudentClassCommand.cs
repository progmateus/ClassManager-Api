using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class TransferStudentsClassCommand : Notifiable, ICommand
  {
    public Guid FromId { get; set; }
    public Guid ToId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(FromId, "FromId", "FromId cannot be null")
      .IsNotNull(ToId, "ToId", "ToId cannot be null")
    );
    }
  }
}