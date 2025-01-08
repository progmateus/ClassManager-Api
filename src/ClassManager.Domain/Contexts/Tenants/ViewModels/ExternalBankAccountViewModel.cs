using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class ExternalBankAccountViewModel
  {
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string StripeExternalBankAccountId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Last4 { get; set; } = string.Empty;
    public string RoutingNumber { get; set; } = string.Empty;
    public EExternalBankAccountStatus Status { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}