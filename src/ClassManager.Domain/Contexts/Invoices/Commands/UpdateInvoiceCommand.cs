using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class UpdateInvoiceCommand : Notifiable, ICommand
{
  public EInvoiceStatus Status { get; private set; }

  public void Validate()
  {
    AddNotifications(new Contract()
      .Requires()
      .IsNotNull(Status, "Status", "Status cannot be null")
    );
  }
}