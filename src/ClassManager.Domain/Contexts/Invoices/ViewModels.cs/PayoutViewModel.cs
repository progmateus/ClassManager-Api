using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.tenants.ViewModels;

public class PayoutViewModel
{
  public Guid Id { get; set; }
  public Guid TenantId { get; set; }
  public string StripePayoutId { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public string Currency { get; set; } = string.Empty;
  public EPayoutStatus Status { get; set; }
  public TenantViewModel? Tenant { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}