using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class DeleteUserHandler
{
  private readonly IUserRepository _userReporitory;
  public DeleteUserHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    var user = await _userReporitory.GetByIdAsync(id, new CancellationToken());

    if (user == null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    user.Delete();

    await _userReporitory.UpdateAsync(user, new CancellationToken());

    return new CommandResult(true, "USER_DELETED", null, null, 204);
  }
}
