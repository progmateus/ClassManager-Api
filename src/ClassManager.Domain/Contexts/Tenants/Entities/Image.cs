using System;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Image : TenantEntity
  {

    protected Image()
    {

    }
    public Image(string name, Guid tenantId)
    {
      Name = name;
      TenantId = tenantId;
    }

    public string Name { get; private set; } = null!;
    public Tenant? Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
  }
}