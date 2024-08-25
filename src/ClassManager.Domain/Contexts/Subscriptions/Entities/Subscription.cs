using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Entities;
public class Subscription : Entity
{
  protected Subscription() { }
  public Subscription(Guid userId, Guid planId, Guid tenantId, DateTime expiresDate)
  {
    UserId = userId;
    TenantPlanId = planId;
    TenantId = tenantId;
    Status = ESubscriptionStatus.ACTIVE;
    ExpiresDate = expiresDate;
  }

  public Guid UserId { get; set; }
  public Guid TenantPlanId { get; set; }
  public Guid TenantId { get; set; }
  public ESubscriptionStatus Status { get; set; }
  public User User { get; set; }
  public TenantPlan TenantPlan { get; set; }
  public Tenant Tenant { get; set; }
  public DateTime ExpiresDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public void ChangePlan(Guid planId)
  {
    TenantPlanId = planId;
  }

  public void ChangeStatus(ESubscriptionStatus status)
  {
    Status = status;
  }
}