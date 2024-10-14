using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class TenantInvoice : TenantEntity
{
  public TenantInvoice(Guid tenantId, Guid planId, decimal amount, ESubscriptionStatus type, ESubscriptionStatus status, DateTime expiresDate)
  {
    TenantId = tenantId;
    PlanId = planId;
    Amount = amount;
    Type = type;
    Status = status;
    ExpiresDate = expiresDate;
  }

  protected TenantInvoice() { }
  public Guid PlanId { get; private set; }
  public decimal Amount { get; private set; }
  public ESubscriptionStatus Type { get; private set; }
  public ESubscriptionStatus Status { get; private set; }
  public DateTime ExpiresDate { get; private set; }
  public Plan? Plan { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetAmount()
  {

  }

  public void SetExpiresDate()
  {

  }
}