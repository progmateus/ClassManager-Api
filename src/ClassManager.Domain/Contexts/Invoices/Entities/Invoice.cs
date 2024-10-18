using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Invoice : TenantEntity
{
  public Invoice(Guid userId, Guid? tenantPlanId, Guid? subscriptionId, Guid? planId, Guid tenantId, EInvoiceTargetType targetType, EInvoiceType type)
  {
    UserId = userId;
    TenantPlanId = tenantPlanId;
    SubscriptionId = subscriptionId;
    PlanId = planId;
    TargetType = targetType;
    Type = type;
    TenantId = tenantId;
    ExpiresAt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
  }

  protected Invoice() { }

  public Guid UserId { get; private set; }
  public Guid? TenantPlanId { get; private set; }
  public Guid? SubscriptionId { get; private set; }
  public Guid? PlanId { get; private set; }
  public decimal Amount { get; private set; }
  public EInvoiceStatus Status { get; private set; } = EInvoiceStatus.PENDING;
  public EInvoiceTargetType TargetType { get; private set; } = EInvoiceTargetType.USER;
  public EInvoiceType Type { get; private set; } = EInvoiceType.USER_SUBSCRIPTION;
  public string? StripeInvoiceId { get; private set; }
  public string? StripeInvoiceUrl { get; private set; }
  public DateTime ExpiresAt { get; private set; }
  public User? User { get; private set; }
  public TenantPlan? TenantPlan { get; private set; }
  public Tenant? Tenant { get; private set; }
  public Plan? Plan { get; private set; }
  public Subscription? Subscription { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public void SetExpiresDate()
  {
    ExpiresAt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
  }

  public void UpdateStatus(EInvoiceStatus status)
  {
    Status = status;
  }
  public void SetStripeInformations(string? stripeInvoiceId, string? stripeInvoiceUrl)
  {
    StripeInvoiceId = stripeInvoiceId;
    StripeInvoiceUrl = stripeInvoiceUrl;
  }
}