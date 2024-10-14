using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class UserInvoice : Entity
{
  public UserInvoice(Guid userId, Guid tenantPlanId, Guid subscriptionId, decimal amount)
  {
    UserId = userId;
    TenantPlanId = tenantPlanId;
    SubscriptionId = subscriptionId;
    Amount = amount;
  }

  protected UserInvoice() { }

  public Guid UserId { get; private set; }
  public Guid TenantPlanId { get; private set; }
  public Guid SubscriptionId { get; private set; }
  public decimal Amount { get; private set; }
  public EInvoiceStatus Status { get; private set; } = EInvoiceStatus.PENDING;
  public DateTime ExpiresDate { get; private set; }
  public User? User { get; private set; }
  public TenantPlan? TenantPlan { get; private set; }
  public Subscription? Subscription { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetExpiresDate()
  {
    ExpiresDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
  }
}