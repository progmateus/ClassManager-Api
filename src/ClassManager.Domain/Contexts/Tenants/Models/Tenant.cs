using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Tenant : Entity
  {
    public Tenant(string name, Document document, Email email)
    {
      Name = name;
      Document = document;
      Email = email;
    }

    protected Tenant()
    {

    }


    public string Name { get; private set; } = null!;
    public Document Document { get; private set; }
    public Email Email { get; private set; }
    public string? Avatar { get; private set; } = string.Empty;
    public ETenantStatus Status { get; private set; } = ETenantStatus.ACTIVE;
    public List<Role> Roles { get; private set; }
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