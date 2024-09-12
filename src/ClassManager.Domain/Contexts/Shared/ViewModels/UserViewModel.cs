using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Shared.ViewModels;

public class UserViewModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Document { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public string? Avatar { get; set; } = string.Empty;
  public EUserStatus Status { get; set; } = EUserStatus.ACTIVE;
  public EUserType Type { get; set; } = EUserType.NORMAL;
  public IList<UsersRoles> UsersRoles { get; set; } = [];
  public IList<TeachersClasses> TeachersClasses { get; set; } = [];
  public IList<StudentsClasses> StudentsClasses { get; set; } = [];
  public IList<Subscription> Subscriptions { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}