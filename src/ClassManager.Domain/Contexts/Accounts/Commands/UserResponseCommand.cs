using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Accounts.Commands;

public class Response : ClassManager.Domain.Contexts.Shared.UseCases.Response
{
  protected Response()
  {
  }

  public Response(
      string message,
      int status,
      IEnumerable<Notification>? notifications = null)
  {
    Message = message;
    Status = status;
    Notifications = notifications;
  }

  public Response(string message, UserResponseData data)
  {
    Message = message;
    Status = 201;
    Notifications = null;
    Data = data;
  }

  public UserResponseData? Data { get; set; }
}

public class UserResponseData
{
  public string FirstName { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string Document { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? Avatar { get; set; }
}