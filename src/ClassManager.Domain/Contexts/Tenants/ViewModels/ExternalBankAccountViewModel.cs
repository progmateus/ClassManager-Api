using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Tenants.ViewModels
{
  public class ExternalBankAccountViewModel
  {
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string StripeExternalBankAccountId { get; private set; } = "";
    public string Name { get; private set; } = "";
    public string Country { get; private set; } = "";
    public string Currency { get; private set; } = "";
    public string Last4 { get; private set; } = "";
    public string RoutingNumber { get; private set; } = "";
    public EExternalBankAccountStatus Status { get; private set; }
    public TenantPlanViewModel? Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
  }
}