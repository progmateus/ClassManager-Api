using Flunt.Notifications;

namespace ClassManager.Shared.Contracts;

public class AuthData
{
  public string Token { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string? Avatar { get; set; } = string.Empty;
  public string[] Roles { get; set; } = Array.Empty<string>();
}