using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.ValueObjects;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class TenantPlan : Entity
  {
    public TenantPlan(string name, string description, int timesOfweek, Guid tenantId)
    {
      Name = name;
      Description = description;
      TimesOfweek = timesOfweek;
      TenantId = tenantId;
    }

    protected TenantPlan()
    {

    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; }
    public int TimesOfweek { get; private set; }
    public decimal Price { get; private set; }
    public Guid TenantId { get; private set; }
    public Tenant? Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

  }
}