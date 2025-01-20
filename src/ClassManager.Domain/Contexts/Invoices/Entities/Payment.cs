using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Payment : TenantEntity
{

  protected Payment() { }

  public Payment(
    string stripePaymentIntentId,
    decimal amount,
    string currency,
    Guid tenantId
  )
  {
    StripePaymentIntentId = stripePaymentIntentId;
    Amount = amount;
    Currency = currency;
    TenantId = tenantId;
  }


  public string StripePaymentIntentId { get; private set; }
  public decimal Amount { get; private set; }
  public string Currency { get; private set; }
  public Tenant? Tenant { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}