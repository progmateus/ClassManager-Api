
using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Plans.ViewModels;

public class PlanViewModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public int StudentsLimit { get; set; }
  public int ClassesLimit { get; set; }
  public decimal Price { get; set; }
  public IList<TenantViewModel> Tenants { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}