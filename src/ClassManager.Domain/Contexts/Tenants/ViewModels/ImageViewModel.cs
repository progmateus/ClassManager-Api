using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class ImageViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Url { get; set; } = String.Empty;
    public Guid TenantId { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}