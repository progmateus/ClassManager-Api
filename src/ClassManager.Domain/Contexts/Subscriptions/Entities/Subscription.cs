using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class Subscription : TenantEntity
{
  protected Subscription() { }

  public Subscription(Guid tenantId, Guid planId)
  {
    PlanId = planId;
    TenantId = tenantId;
    TargetType = ETargetType.TENANT;
  }
  public Subscription(Guid tenantId, Guid tenantPlanId, Guid userId)
  {
    TenantId = tenantId;
    TenantPlanId = tenantPlanId;
    UserId = userId;
    TargetType = ETargetType.USER;
  }



  public Guid UserId { get; private set; }
  public Guid? PlanId { get; private set; }
  public Guid? TenantPlanId { get; private set; }
  public Guid? LatestInvoiceId { get; private set; }
  public ESubscriptionStatus Status { get; private set; } = ESubscriptionStatus.INCOMPLETE;
  public ETargetType TargetType { get; private set; } = ETargetType.USER;
  public string? StripeSubscriptionId { get; private set; }
  public DateTime CurrentPeriodStart { get; private set; }
  public DateTime CurrentPeriodEnd { get; private set; }
  public User? User { get; private set; }
  public Plan? Plan { get; private set; }
  public TenantPlan? TenantPlan { get; private set; }
  public Tenant? Tenant { get; private set; }
  public Invoice? LatestInvoice { get; private set; }
  public IList<Invoice> Invoices { get; private set; } = [];
  public DateTime ExpiresDate { get; private set; }
  public DateTime? CanceledAt { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetTenantPlan(Guid tenantPlanId)
  {
    TenantPlanId = tenantPlanId;
  }

  public void SetPlan(Guid planId)
  {
    PlanId = planId;
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

  public void SetLatestInvoice(Guid invoiceId)
  {
    LatestInvoiceId = invoiceId;
  }
  public void SetCanceledAt(DateTime? canceledAt)
  {
    CanceledAt = canceledAt;
  }
}