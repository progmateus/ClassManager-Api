using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Contexts.Auth.Services;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Contracts;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class AuthHandler :
  Notifiable,
  IHandler<AuthCommand>
{
  private readonly IUserRepository _userReporitory;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly ISubscriptionRepository _subscriptionsrepository;
  private IMapper _mapper;
  public AuthHandler(
    IUserRepository userRepository,
    IUsersRolesRepository usersRolesRepository,
    ISubscriptionRepository subscriptionsrepository,
    IMapper mapper
  )
  {
    _userReporitory = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionsrepository = subscriptionsrepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(AuthCommand command)
  {

    try
    {
      command.Validate();
      if (command.Invalid)
      {
        AddNotifications(command);
        return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, command.Notifications);
      }
    }
    catch
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    var user = await _userReporitory.GetByEmailAsync(command.Email, default);

    if (user is null)
    {
      return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, null, 401);
    }

    if (!user.Password.Challenge(command.Password))
      return new CommandResult(false, "ERR_INVALID_CREDENTIALS", null, null, 401);

    try
    {
      if (user.Status != EUserStatus.ACTIVE)
        return new CommandResult(false, "ERR_USER_INACTIVE", null, null, 401);
    }
    catch
    {
      return new CommandResult(false, "Internal server error", null, null, 500);
    }

    var userRoles = _mapper.Map<List<UsersRolesViewModel>>(await _usersRolesRepository.FindByUserId(user.Id));
    var userSubscriptions = _mapper.Map<List<SubscriptionViewModel>>(await _subscriptionsrepository.ListSubscriptions([user.Id], []));

    var tokenService = new TokenService();

    var authData = new AuthData
    {
      Id = user.Id.ToString(),
      User = _mapper.Map<UserViewModel>(user),
    };

    authData.Token = tokenService.Create(_mapper.Map<UserViewModel>(user), Configuration.Secrets.Token, DateTime.UtcNow.AddHours(2));
    authData.User.Subscriptions = userSubscriptions;
    authData.User.UsersRoles = userRoles;

    return new CommandResult(true, "USER_GOTTEN", authData, null, 200);
  }
}