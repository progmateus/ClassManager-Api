using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class TenantSocialViewModel
  {
    public Guid Id { get; set; }
    public int type { get; set; }
    public string url { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid TenantId { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}