using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Roles.Entities;

public class Role : Entity
{
  protected Role() { }

  public Role(string name, string description = "")
  {
    Name = name;
    Description = description;
    CreatedAt = DateTime.UtcNow;
    Updatedat = DateTime.UtcNow;
  }

  public string Name { get; set; }
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime Updatedat { get; set; }
  public List<User> Users { get; } = new();
  public List<UsersRoles> UsersRoles { get; } = new();
  public void ChangeRole(string name, string description)
  {
    Name = name;
    Description = description;
  }
}