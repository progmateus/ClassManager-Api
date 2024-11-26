using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Roles.ViewModels;

public class UsersRolesViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid TenantId { get; set; }
  public Guid RoleId { get; set; }
  public UserProfileViewModel? User { get; set; }
  public RoleViewModel? Role { get; set; }
  public TenantViewModel? Tenant { get; set; }
}