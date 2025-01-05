using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class StripeCustomer : TenantEntity
{
  protected StripeCustomer() { }

  public StripeCustomer(string stripeCustomerId, Guid tenantId, Guid userId)
  {
    UserId = userId;
    TenantId = tenantId;
    StripeCustomerId = stripeCustomerId;
    TargetType = ETargetType.USER;
  }

  public StripeCustomer(string stripeCustomerId, Guid tenantId)
  {
    TenantId = tenantId;
    StripeCustomerId = stripeCustomerId;
    TargetType = ETargetType.TENANT;
  }

  public Guid UserId { get; private set; }
  public string StripeCustomerId { get; private set; }
  public ETargetType TargetType { get; private set; } = ETargetType.USER;
  public User? User { get; private set; }
  public Tenant? Tenant { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}