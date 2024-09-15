using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Shared.Contracts;

public class AuthData
{
  public string Token { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public UserViewModel User { get; set; } = new();
}