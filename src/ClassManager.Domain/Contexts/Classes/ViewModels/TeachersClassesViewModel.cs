using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Classes.ViewModels
{
  public class TeachersClassesViewModel
  {
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ClassId { get; private set; }
    public ClassViewModel? Class { get; private set; } = null!;
    public UserViewModel? User { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
  }
}