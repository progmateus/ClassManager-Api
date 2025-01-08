using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.tenants.ViewModels;

public class PayoutViewModel
{
  public Guid Id { get; private set; }
  public Guid TenantId { get; private set; }
  public string StripePayoutId { get; private set; } = "";
  public decimal Amount { get; private set; }
  public string Currency { get; private set; }
  public EPayoutStatus Status { get; private set; }
  public TenantViewModel? Tenant { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}