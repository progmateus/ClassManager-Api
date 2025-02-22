
using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Subscriptions.ViewModels;

public class SubscriptionViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid TenantPlanId { get; set; }
  public Guid PlanId { get; set; }
  public Guid TenantId { get; set; }
  public Guid? LatestInvoiceId { get; set; }
  public int Status { get; set; }
  public DateTime CurrentPeriodStart { get; set; }
  public DateTime CurrentPeriodEnd { get; set; }
  public UserViewModel? User { get; set; }
  public TenantViewModel? Tenant { get; set; }
  public TenantPlanViewModel? TenantPlan { get; set; }
  public PlanViewModel? Plan { get; set; }
  public TenantPlanViewModel? NextTenantPlan { get; set; }
  public PlanViewModel? NextPlan { get; set; }
  public IList<InvoiceViewModel> Invoices { get; set; } = [];
  public InvoiceViewModel? LatestInvoice { get; set; }
  public DateTime ExpiresDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}