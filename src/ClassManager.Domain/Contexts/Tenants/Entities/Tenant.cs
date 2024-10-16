using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Contexts.Invoices.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Tenant : Entity
  {
    protected Tenant()
    {

    }
    public Tenant(string name, Document document, string username, string description, Email email, Guid userId)
    {
      Name = name;
      Document = document;
      Username = username;
      Description = description;
      Email = email;
      UserId = userId;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;
    public string Username { get; private set; }
    public string? Description { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public ETenantStatus Status { get; private set; } = ETenantStatus.ACTIVE;
    public Guid? UserId { get; private set; }
    public Guid? PlanId { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public DateTime? ExpiresDate { get; private set; }
    public Plan? Plan { get; private set; }
    public User? User { get; private set; }
    public List<Role> Roles { get; private set; } = [];
    public List<UsersRoles> UsersRoles { get; private set; } = [];
    public List<TenantPlan> TenantPlans { get; private set; } = [];
    public List<Link> Links { get; private set; } = [];
    public List<Subscription> Subscriptions { get; private set; } = [];
    public List<TimeTable> TimesTables { get; private set; } = [];
    public List<Class> Classes { get; private set; } = [];
    public IList<Invoice> Invoices { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Update(string name, Email email, Document document, string? description)
    {
      AddNotifications(email, document);
      Name = name;
      Email = email;
      Document = document;
      Description = description;
    }

    public void UpdateStatus(ETenantStatus status)
    {
      Status = status;
    }

    public void SetStripeCustomerId(string stripeCustomerId)
    {
      StripeCustomerId = stripeCustomerId;
    }
  }
}