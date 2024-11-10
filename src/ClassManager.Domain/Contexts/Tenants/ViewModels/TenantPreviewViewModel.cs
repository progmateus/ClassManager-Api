using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.tenants.ViewModels;

public class TenantViewModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Username { get; set; }
  public string? Description { get; set; }
  public string? Email { get; set; }
  public string? Document { get; set; }
  public string? Avatar { get; set; }
  public int Status { get; set; }
  public int Type { get; set; }
  public List<ClassViewModel> Classes { get; set; } = [];
  public List<Link> Links { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}