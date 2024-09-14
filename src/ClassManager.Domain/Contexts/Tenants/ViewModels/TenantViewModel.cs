using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;

public class TenantViewModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Username { get; set; }
  public string? Description { get; set; }
  public string? Email { get; set; }
  public string? Document { get; set; }
  public string? Avatar { get; set; }
  public int Status { get; set; }
  public int Type { get; set; }
  public Guid UserId { get; set; }
  public Guid PlanId { get; set; }
  public DateTime ExpiresDate { get; set; }
  public object? Plan { get; set; }
  public UserViewModel? User { get; set; }
  public List<Role> Roles { get; set; } = [];
  public List<UsersRoles> UsersRoles { get; } = [];
  public List<TenantPlan> TenantPlans { get; } = [];
  public List<Subscription> Subscriptions { get; } = [];
  public List<Class> Classes { get; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}