using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class TenantPlanViewModel
  {

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int TimesOfweek { get; set; }
    public decimal Price { get; set; }
    public Guid TenantId { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public ICollection<SubscriptionViewModel> Subscriptions { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}