using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Roles.ViewModels;

public class RoleViewModel
{
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime Updatedat { get; set; }
  public List<UserViewModel> Users { get; set; } = [];
  public List<UsersRolesViewModel> UsersRoles { get; set; } = [];
}