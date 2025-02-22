using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Tenants.Entities
{
  public class Link : TenantEntity
  {

    protected Link()
    {

    }
    public Link(string url, ESocialType type, Guid tenantId)
    {
      Url = url;
      Type = type;
      TenantId = tenantId;
    }

    public string Url { get; private set; } = null!;
    public ESocialType Type { get; private set; }
    public Tenant? Tenant { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void UpdateUrl(string url)
    {
      Url = url;
    }
  }
}