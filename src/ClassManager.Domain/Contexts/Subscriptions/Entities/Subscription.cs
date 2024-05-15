using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class Subscription : Entity
{
  protected Subscription() { }
  public Subscription(Guid userId, Guid planId, Guid nextPlanId, DateTime expiresDate)
  {
    UserId = userId;
    TenantPlanId = planId;
    NextPlanId = nextPlanId;
    Status = ESubscriptionStatus.ACTIVE;
    ExpiresDate = expiresDate;
  }

  public Guid UserId { get; set; }
  public Guid TenantPlanId { get; set; }
  public Guid NextPlanId { get; set; }
  public ESubscriptionStatus Status { get; set; }
  public User User { get; set; }
  public TenantPlan TenantPlan { get; set; }
  public TenantPlan NextTenantPlan { get; set; }
  public DateTime ExpiresDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}