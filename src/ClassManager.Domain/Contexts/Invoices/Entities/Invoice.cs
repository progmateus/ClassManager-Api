using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Entities;
public class Invoice : TenantEntity
{
  public Invoice(
    Guid userId,
    Guid? tenantPlanId,
    Guid? subscriptionId,
    Guid? planId,
    Guid tenantId,
    decimal amount,
    ETargetType targetType,
    EInvoiceType type,
    string stripeInvoiceId,
    string stripeInvoiceUrl,
    string stripeInvoiceNumber
  )
  {
    UserId = userId;
    TenantPlanId = tenantPlanId;
    SubscriptionId = subscriptionId;
    PlanId = planId;
    TargetType = targetType;
    Type = type;
    TenantId = tenantId;
    Amount = amount;
    StripeInvoiceId = stripeInvoiceId;
    StripeInvoiceUrl = stripeInvoiceUrl;
    StripeInvoiceNumber = stripeInvoiceNumber;
    ExpiresAt = DateTime.Now.AddMonths(1);
  }

  protected Invoice() { }

  public Guid UserId { get; private set; }
  public Guid? TenantPlanId { get; private set; }
  public Guid? SubscriptionId { get; private set; }
  public Guid? PlanId { get; private set; }
  public decimal Amount { get; private set; }
  public EInvoiceStatus Status { get; private set; } = EInvoiceStatus.OPEN;
  public ETargetType TargetType { get; private set; } = ETargetType.USER;
  public EInvoiceType Type { get; private set; } = EInvoiceType.SUBSCRIPTION;
  public string? StripeInvoiceId { get; private set; }
  public string? StripeInvoiceUrl { get; private set; }
  public string? StripeInvoiceNumber { get; private set; }
  public DateTime ExpiresAt { get; private set; }
  public User? User { get; private set; }
  public Payment? Payment { get; private set; }
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
  public void SetStripeInformations(string? stripeInvoiceId, string? stripeInvoiceUrl, string? stripeInvoiceNumber)
  {
    StripeInvoiceId = stripeInvoiceId ?? StripeInvoiceId;
    StripeInvoiceUrl = stripeInvoiceUrl ?? StripeInvoiceUrl;
    StripeInvoiceNumber = stripeInvoiceNumber ?? StripeInvoiceNumber;
  }
}