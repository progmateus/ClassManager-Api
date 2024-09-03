using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class GetUserByUsernameHandler
{
  private readonly IUserRepository _userReporitory;
  public GetUserByUsernameHandler(
    IUserRepository userRepository
    )
  {
    _userReporitory = userRepository;
  }
  public async Task<ICommandResult> Handle(string username)
  {
    var user = await _userReporitory.GetByUsernameAsync(username, new CancellationToken());

    return new CommandResult(true, "USER_GOTTEN", user, null, 200);
  }
}
