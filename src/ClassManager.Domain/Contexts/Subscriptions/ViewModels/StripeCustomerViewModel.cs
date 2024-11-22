
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Subscriptions.ViewModels;

public class StripeCustomerViewModel
{
  public Guid UserId { get; set; }
  public string StripeCustomerId { get; set; } = string.Empty;
  public int Type { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public UserViewModel? User { get; set; }
  public TenantViewModel? Tenant { get; set; }
}