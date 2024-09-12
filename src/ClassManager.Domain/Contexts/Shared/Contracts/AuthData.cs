using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Shared.Contracts;

public class AuthData
{
  public string Token { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public string Document { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string? Avatar { get; set; } = string.Empty;
  public string[] Roles { get; set; } = Array.Empty<string>();
  public IList<UsersRoles> UsersRoles { get; set; } = [];
  public IList<TeachersClasses> TeachersClasses { get; set; } = [];
  public IList<StudentsClasses> StudentsClasses { get; set; } = [];
  public IList<Subscription> Subscriptions { get; set; } = [];
}