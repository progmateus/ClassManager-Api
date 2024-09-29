using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class GetUserByUsernameHandler
{
  private readonly IUserRepository _userReporitory;
  private readonly IMapper _mapper;
  public GetUserByUsernameHandler(
    IUserRepository userRepository,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(string username)
  {
    var user = _mapper.Map<UserViewModel>(await _userReporitory.GetByUsernameAsync(username, new CancellationToken()));

    return new CommandResult(true, "USER_GOTTEN", user, null, 200);
  }
}
