using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.ViewModels;
using ClassManager.Shared.Commands;
namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class GetUserProfileHandler
{
  private readonly IUserRepository _userReporitory;
  public GetUserProfileHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    var user = await _userReporitory.GetByIdWithIncludeAsync(id, new CancellationToken());

    if (user == null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var userModel = new UserViewModel
    {
      Id = user.Id,
      Name = user.Name.ToString(),
      FirstName = user.Name.FirstName,
      LastName = user.Name.LastName,
      Email = user.Email.ToString(),
      Document = user.Document.ToString(),
      Avatar = user.Avatar,
      Username = user.Username,
      Status = user.Status,
      Type = user.Type,
      Subscriptions = user.Subscriptions,
      UsersRoles = user.UsersRoles,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt,
    };

    return new CommandResult(true, "USER_GOTTEN", userModel, null, 200);
  }
}
