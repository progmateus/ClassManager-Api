using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class CreateTenantInvoiceCommand : Notifiable, ICommand
{
  public Guid TenantId { get; private set; }

  public void Validate()
  {
    AddNotifications(new Contract()
      .Requires()
      .IsNotNull(TenantId, "TenantId", "TenantId cannot be null")
    );
  }
}