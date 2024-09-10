using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Shared.ViewModels;

public class UserViewModel
{
  public string Name { get; set; } = string.Empty;
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Document { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public string? Avatar { get; set; } = string.Empty;
  public EUserStatus Status { get; set; } = EUserStatus.ACTIVE;
  public EUserType Type { get; set; } = EUserType.NORMAL;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}