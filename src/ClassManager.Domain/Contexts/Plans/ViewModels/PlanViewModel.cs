namespace ClassManager.Domain.Contexts.Plan.ViewModels;

public class PlanViewModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  public int StudentsLimit { get; set; }
  public int ClassesLimit { get; set; }
  public decimal Price { get; set; }
  public List<TenantViewModel> Tenants { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}