using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class TenantInvoice : TenantEntity
{
  public TenantInvoice(Guid tenantId, Guid planId, decimal amount)
  {
    TenantId = tenantId;
    PlanId = planId;
    Amount = amount;
  }

  protected TenantInvoice() { }
  public Guid PlanId { get; private set; }
  public decimal Amount { get; private set; }
  public EInvoiceStatus Status { get; private set; } = EInvoiceStatus.PENDING;
  public DateTime ExpiresDate { get; private set; }
  public Plan? Plan { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetExpiresDate()
  {
    ExpiresDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
  }

  public void UpdateStatus(EInvoiceStatus status)
  {
    Status = status;
  }
}