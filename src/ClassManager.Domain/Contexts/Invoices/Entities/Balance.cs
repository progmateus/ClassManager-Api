using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Balance : TenantEntity
{
  public Balance(
    Guid tenantId
  )
  {
    TenantId = tenantId;
  }

  protected Balance() { }

  public decimal Available { get; private set; } = 0;
  public decimal Pending { get; private set; } = 0;
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetAvailable(decimal amount)
  {
    Available = amount;
  }

  public void SetPending(decimal amount)
  {
    Pending = amount;
  }
}