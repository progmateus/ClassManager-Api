using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.Roles.Entities;

public class Role : Entity
{
  protected Role() { }

  public Role(string name, string description = "")
  {
    Name = name;
    Description = description;
    CreatedAt = DateTime.Now;
    Updatedat = DateTime.Now;
  }

  public string Name { get; set; }
  public string? Description { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime Updatedat { get; set; }


  public void ChangeRole(string name, string description)
  {
    Name = name;
    Description = description;
  }
}