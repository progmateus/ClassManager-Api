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

    if (await _userReporitory.GetByIdAsync(id, default) == null)
    {
      return new CommandResult(false, "User not found", null, null, 404);
    }


    await _userReporitory.DeleteAsync(id, default);

    return new CommandResult(true, "User deleted", null, null, 204);
  }
}
