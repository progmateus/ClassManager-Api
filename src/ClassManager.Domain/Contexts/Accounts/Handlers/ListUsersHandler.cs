using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersHandler
{
  private readonly IUserRepository _userReporitory;
  private readonly IMapper _mapper;
  public ListUsersHandler(
    IUserRepository userRepository,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle()
  {
    var users = _mapper.Map<List<UserPreviewViewModel>>(await _userReporitory.GetAllAsync(default));
    return new CommandResult(true, "USERS_LISTED", users, null, 201);
  }
}
