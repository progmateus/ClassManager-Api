using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Roles.ViewModels;

public class UsersRolesPreviewViewModel
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid TenantId { get; set; }
  public Guid RoleId { get; set; }
  public UserPreviewViewModel? User { get; set; }
  public RoleViewModel? Role { get; set; }
  public TenantPreviewViewModel? Tenant { get; set; }
}