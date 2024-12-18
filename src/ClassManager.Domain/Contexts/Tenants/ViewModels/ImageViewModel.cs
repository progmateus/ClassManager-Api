using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class ImageViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public Guid TenantId { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}