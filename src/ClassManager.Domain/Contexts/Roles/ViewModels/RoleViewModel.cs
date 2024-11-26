using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Roles.ViewModels;

public class RoleViewModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime Updatedat { get; set; }
  public List<UserPreviewViewModel> Users { get; set; } = [];
  public List<UsersRolesPreviewViewModel> UsersRoles { get; set; } = [];
}