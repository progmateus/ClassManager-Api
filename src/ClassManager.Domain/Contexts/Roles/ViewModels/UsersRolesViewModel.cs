using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Roles.ViewModels;

public class UsersRolesViewModel
{
  public Guid UserId { get; set; }
  public Guid RoleId { get; set; }
  public User? User { get; set; }
  public RoleViewModel? Role { get; set; }
  public TenantViewModel? Tenant { get; set; }
}