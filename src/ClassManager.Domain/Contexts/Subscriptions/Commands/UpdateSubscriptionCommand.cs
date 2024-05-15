using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.Roles.Commands;

public class UpdateSubscriptionCommand : Notifiable, ICommand
{
  public ESubscriptionStatus Status { get; set; }
  public void Validate()
  {
    AddNotifications(new Contract()
    .Requires()
    .IsNotNull(Status, "UpdateSubscriptionCommand.Status", "Status not null")
    );
  }
}