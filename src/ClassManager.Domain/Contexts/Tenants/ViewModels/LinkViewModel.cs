using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class LinkViewModel
  {
    public Guid Id { get; set; }
    public string Url { get; private set; } = null!;
    public int Type { get; private set; }
    public Guid TenantId { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}