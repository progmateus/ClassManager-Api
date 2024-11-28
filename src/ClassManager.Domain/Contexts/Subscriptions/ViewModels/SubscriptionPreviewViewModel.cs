
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Subscriptions.ViewModels;

public class SubscriptionPreviewViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid TenantPlanId { get; set; }
  public Guid TenantId { get; set; }
  public int Status { get; set; }
  public UserPreviewViewModel? User { get; set; }
  public TenantPlanViewModel? TenantPlan { get; set; }
  public TenantPreviewViewModel? Tenant { get; set; }
  public List<InvoiceViewModel> Invoices { get; set; } = [];
  public DateTime ExpiresDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}