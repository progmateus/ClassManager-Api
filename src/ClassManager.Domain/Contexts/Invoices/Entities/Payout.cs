using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Payout : TenantEntity
{

  protected Payout() { }

  public Payout(
    string stripePayoutId,
    decimal amount,
    string currency,
    Guid tenantId,
    EPayoutStatus status
  )
  {
    StripePayoutId = stripePayoutId;
    Amount = amount;
    Currency = currency;
    TenantId = tenantId;
    Status = status;
  }


  public string StripePayoutId { get; private set; }
  public decimal Amount { get; private set; }
  public string Currency { get; private set; }
  public EPayoutStatus Status { get; private set; } = EPayoutStatus.PENDING;
  public Tenant? Tenant { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}