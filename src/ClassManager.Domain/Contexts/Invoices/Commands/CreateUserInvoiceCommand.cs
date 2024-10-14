using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class CreateUserInvoiceCommand : Notifiable, ICommand
{
  public Guid SubscriptionId { get; private set; }

  public void Validate()
  {
    AddNotifications(new Contract()
      .Requires()
      .IsNotNull(SubscriptionId, "SubscriptionId", "SubscriptionId cannot be null")
    );
  }
}