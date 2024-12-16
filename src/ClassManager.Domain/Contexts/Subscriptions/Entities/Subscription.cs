using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class Subscription : TenantEntity
{
  protected Subscription() { }
  public Subscription(Guid userId, Guid planId, Guid tenantId)
  {
    UserId = userId;
    TenantId = tenantId;
    TenantPlanId = planId;
    Status = ESubscriptionStatus.INCOMPLETE;
  }

  public Guid UserId { get; private set; }
  public Guid TenantPlanId { get; private set; }
  public ESubscriptionStatus Status { get; private set; } = ESubscriptionStatus.INCOMPLETE;
  public string? StripeSubscriptionId { get; private set; }
  public DateTime CurrentPeriodStart { get; private set; }
  public DateTime CurrentPeriodEnd { get; private set; }
  public User? User { get; private set; }
  public TenantPlan? TenantPlan { get; private set; }
  public Tenant? Tenant { get; private set; }
  public IList<Invoice> Invoices { get; private set; } = [];
  public DateTime ExpiresDate { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void ChangePlan(Guid planId)
  {
    TenantPlanId = planId;
  }

  public void SetStatus(ESubscriptionStatus status)
  {
    Status = status;
  }

  public void SetStripeSubscriptionId(string stripeSubscriptionId)
  {
    StripeSubscriptionId = stripeSubscriptionId;
  }

  public void SetCurrentPeriod(DateTime? currentPeriodStart, DateTime? currentPeriodEnd)
  {
    CurrentPeriodStart = currentPeriodStart ?? CurrentPeriodStart;
    CurrentPeriodEnd = currentPeriodEnd ?? CurrentPeriodEnd;
  }
}