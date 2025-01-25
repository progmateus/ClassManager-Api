using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Payment : TenantEntity
{

  protected Payment() { }

  public Payment(
    decimal amount,
    string currency,
    Guid tenantId,
    Guid invoiceId
  )
  {
    Amount = amount;
    Currency = currency;
    TenantId = tenantId;
    InvoiceId = invoiceId;
  }


  public decimal Amount { get; private set; }
  public string Currency { get; private set; }
  public Guid InvoiceId { get; private set; }
  public Tenant? Tenant { get; private set; }
  public Invoice? Invoice { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}