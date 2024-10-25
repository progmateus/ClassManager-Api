using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class StripeCustomer : TenantEntity
{
  protected StripeCustomer() { }
  public StripeCustomer(Guid userId, Guid tenantId)
  {
    UserId = userId;
    TenantId = tenantId;
  }

  public Guid UserId { get; private set; }
  public User? User { get; private set; }
  public Tenant? Tenant { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
}