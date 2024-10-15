using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class CreateInvoiceCommand : Notifiable, ICommand
{
  public Guid SubscriptionId { get; private set; }
  public Guid PlanId { get; private set; }
  public Guid UserId { get; private set; }
  public EInvoiceTargetType TargetType { get; private set; }
  public EInvoiceType Type { get; private set; }
  public DateTime ExpiresAt { get; private set; }


  public void Validate()
  {
    AddNotifications(new Contract()
      .Requires()
      .IsNotNull(SubscriptionId, "SubscriptionId", "SubscriptionId cannot be null")
    );
  }
}