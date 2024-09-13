using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Roles.Entities;

public class UsersRoles : TenantEntity
{
  protected UsersRoles() { }

  public UsersRoles(Guid userId, Guid roleId, Guid tenantId)
  {
    UserId = userId;
    RoleId = roleId;
    TenantId = tenantId;
  }

  public Guid UserId { get; private set; }
  public Guid RoleId { get; private set; }
  public User? User { get; private set; }
  public Role? Role { get; private set; }
  public Tenant? Tenant { get; private set; }
}