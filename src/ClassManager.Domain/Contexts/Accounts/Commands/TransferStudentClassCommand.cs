using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Accounts.Commands
{
  public class TransferClassStudentsCommand : Notifiable, ICommand
  {
    public Guid ToId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .IsNotNull(ToId, "ToId", "ToId cannot be null")
    );
    }
  }
}