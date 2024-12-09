namespace ClassManager.Domain.Contexts.Classes.ViewModels
{
  public class ClassPreviewViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid TenantId { get; set; }
    public string? Description { get; set; }
    public int TeachersCount { get; set; }
    public int StudentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}