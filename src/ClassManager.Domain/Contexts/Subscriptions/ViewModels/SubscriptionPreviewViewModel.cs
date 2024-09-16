
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
  public UserViewModel? User { get; set; }
  public TenantPlanViewModel? TenantPlan { get; set; }
  public TenantViewModel? Tenant { get; set; }
  public DateTime ExpiresDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}