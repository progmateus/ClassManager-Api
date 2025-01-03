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
using ClassManager.Domain.Contexts.Addresses.Entites;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Tenant : Entity
  {
    protected Tenant()
    {

    }
    public Tenant(string name, Document document, string username, string description, Email email, Guid userId, Guid planId)
    {
      Name = name;
      Document = document;
      Username = username;
      Description = description;
      Email = email;
      UserId = userId;
      PlanId = planId;
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
    public Guid PlanId { get; private set; }
    public string? StripeAccountId { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public bool StripeChargesEnabled { get; private set; } = false;
    public Plan? Plan { get; private set; }
    public User? User { get; private set; }
    public IList<Address> Addresses { get; private set; } = [];
    public IList<Role> Roles { get; private set; } = [];
    public IList<UsersRoles> UsersRoles { get; private set; } = [];
    public IList<TenantPlan> TenantPlans { get; private set; } = [];
    public IList<Link> Links { get; private set; } = [];
    public IList<Image> Images { get; private set; } = [];
    public IList<Subscription> Subscriptions { get; private set; } = [];
    public IList<TimeTable> TimesTables { get; private set; } = [];
    public IList<Class> Classes { get; private set; } = [];
    public IList<Invoice> Invoices { get; private set; } = [];
    public IList<StripeCustomer> StripeCustomers { get; private set; } = [];
    public IList<Payout> Payouts { get; private set; } = [];
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

    public void SetStripeInformations(string stripeAccountId, string stripeCustomerId)
    {
      StripeAccountId = stripeAccountId;
      StripeCustomerId = stripeCustomerId;
    }

    public void UpdateChargesEnabled(bool chargesEnabled)
    {
      StripeChargesEnabled = chargesEnabled;
    }

    public void SetAvatar(string avatar)
    {
      Avatar = avatar;
    }
  }
}