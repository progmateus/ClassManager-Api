using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Domain.Contexts.Classes.ViewModels
{
  public class StudentsClassesViewModel
  {
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ClassId { get; private set; }
    public ClassViewModel? Class { get; private set; } = null!;
    public UserPreviewViewModel? User { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
  }
}