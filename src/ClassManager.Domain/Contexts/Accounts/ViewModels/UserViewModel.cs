using ClassManager.Domain.Contexts.Shared.Enums;

public class UserViewModel
{
  public Guid Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? Username { get; set; }
  public string? Avatar { get; set; }
  public int? Status { get; set; }
  public int? Type { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}