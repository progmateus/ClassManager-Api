using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.tenants.ViewModels;

public class InvoiceViewModel
{
  public Guid UserId { get; set; }
  public Guid? TenantPlanId { get; set; }
  public Guid? SubscriptionId { get; set; }
  public Guid? PlanId { get; set; }
  public decimal Amount { get; set; }
  public EInvoiceStatus Status { get; set; }
  public EInvoiceTargetType TargetType { get; set; }
  public EInvoiceType Type { get; set; }
  public string? StripeInvoiceId { get; set; }
  public string? StripeInvoiceUrl { get; set; }
  public string? StripeInvoiceNumber { get; set; }
  public DateTime ExpiresAt { get; set; }
  public UserViewModel? User { get; set; }
  public TenantPlanViewModel? TenantPlan { get; set; }
  public TenantViewModel? Tenant { get; set; }
  public PlanViewModel? Plan { get; set; }
  public SubscriptionViewModel? Subscription { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}