using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Tenant : Entity
  {
    protected Tenant()
    {

    }
    public Tenant(string name, Document document, string username, Email email)
    {
      Name = name;
      Document = document;
      Username = username;
      Email = email;
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;
    public string Username { get; private set; }
    public Document Document { get; private set; }
    public Email Email { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public ETenantStatus Status { get; private set; } = ETenantStatus.ACTIVE;
    public Guid? PlanId { get; private set; }
    public DateTime? ExpiresDate { get; private set; }
    public Plan? Plan { get; private set; }
    public List<Role> Roles { get; private set; }
    public List<UsersRoles> UsersRoles { get; }
    public List<TenantPlan> TenantPlans { get; }
    public List<Class> Classes { get; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void ChangeTenant(string name, Email email, Document document)
    {
      AddNotifications(email, document);
      Name = name;
      Email = email;
      Document = document;
    }
  }
}