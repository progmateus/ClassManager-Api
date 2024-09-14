using ClassManager.Domain.Contexts.Classes.Entities;

public class ClassViewModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public Guid TenantId { get; private set; }
  public string? Description { get; private set; } = null!;
  public string BusinessHour { get; private set; } = null!;
  public TenantViewModel Tenant { get; set; } = null!;
  public List<UserViewModel> Users { get; set; } = [];
  public List<TeachersClasses> TeachersClasses { get; set; } = [];
  public List<StudentsClasses> StudentsClasses { get; set; } = [];
  public List<ClassDay> ClassDays { get; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}