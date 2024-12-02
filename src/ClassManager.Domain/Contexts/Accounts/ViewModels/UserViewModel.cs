using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.Users.ViewModels;

public class UserViewModel
{
  public Guid Id { get; set; }
  public string? Name { get => $"{FirstName} {LastName}"; }
  public string? FirstName { get; set; } = String.Empty;
  public string? LastName { get; set; } = String.Empty;
  public string? Document { get; set; } = String.Empty;
  public string? Email { get; set; } = String.Empty;
  public string? Username { get; set; } = String.Empty;
  public string? Phone { get; set; } = String.Empty;
  public string? Avatar { get; set; } = String.Empty;
  public int? Status { get; set; }
  public int? Type { get; set; }
  public IList<RoleViewModel> Roles { get; set; } = [];
  public IList<UsersRolesViewModel> UsersRoles { get; set; } = [];
  public IList<ClassViewModel> Classes { get; set; } = [];
  public IList<TeachersClassesViewModel> TeachersClasses { get; set; } = [];
  public IList<StudentsClassesViewModel> StudentsClasses { get; set; } = [];
  public IList<SubscriptionViewModel> Subscriptions { get; set; } = [];
  public IList<BookingViewModel> Bookings { get; set; } = [];
  public IList<TenantViewModel> Tenants { get; set; } = [];
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}