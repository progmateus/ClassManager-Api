using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Roles.Entities;

public class Role : Entity
{
  protected Role() { }

  public Role(string name, Guid tenantId, string description = "")
  {
    Name = name;
    Description = description;
    TenantId = tenantId;
    CreatedAt = DateTime.Now;
    Updatedat = DateTime.Now;
  }

  public string Name { get; set; }
  public string? Description { get; set; }
  public Guid TenantId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime Updatedat { get; set; }
  public Tenant Tenant { get; set; }
  public List<User> Users { get; } = new();
  public void ChangeRole(string name, string description)
  {
    Name = name;
    Description = description;
  }
}