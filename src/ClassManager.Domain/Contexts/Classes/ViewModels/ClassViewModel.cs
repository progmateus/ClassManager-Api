using ClassManager.Domain.Contexts.Addresses.ViewModels;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Classes.ViewModels
{
  public class ClassViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid TenantId { get; set; }
    public Guid? AddressId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public TenantViewModel? Tenant { get; set; }
    public AddressViewModel? Address { get; set; }
    public IList<TeachersClassesViewModel> TeachersClasses { get; set; } = [];
    public IList<StudentsClassesViewModel> StudentsClasses { get; set; } = [];
    public IList<ClassDayViewModel> ClassDays { get; set; } = [];
  }
}