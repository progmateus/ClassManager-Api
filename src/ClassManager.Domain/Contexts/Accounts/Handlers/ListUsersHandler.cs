using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersHandler
{
  private readonly IUserRepository _userReporitory;
  public ListUsersHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }
  public async Task<ICommandResult> Handle()
  {
    var users = await _userReporitory.GetAllAsync(default);

    return new CommandResult(true, "User listed", users, null, 201);
  }
}
