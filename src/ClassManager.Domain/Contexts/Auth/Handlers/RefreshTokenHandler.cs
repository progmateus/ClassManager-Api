using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Contexts.Auth.Services;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Contracts;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class RefreshTokenHandler :
  Notifiable
{
  private readonly IUserTokenRepository _userTokenReposiotry;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly ISubscriptionRepository _subscriptionsrepository;
  private IMapper _mapper;
  public RefreshTokenHandler(
    IUserTokenRepository userTokenRepository,
    IUsersRolesRepository usersRolesRepository,
    ISubscriptionRepository subscriptionsrepository,
    IMapper mapper
  )
  {
    _userTokenReposiotry = userTokenRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionsrepository = subscriptionsrepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, RefreshTokenCommand command)
  {

    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var userToken = await _userTokenReposiotry.FindbyUserIdAndRefreshToken(loggedUserId, command.RefreshToken, default);

    if (userToken is null)
    {
      return new CommandResult(false, "ERR_REFRESH_TOKEN_NOT_FOUND", null, null, 401);
    }

    var tokenService = new TokenService();

    var espiresDate = DateTime.UtcNow.AddHours(30);

    var refreshToken = tokenService.Create(_mapper.Map<UserViewModel>(userToken.User), Configuration.Secrets.RefreshToken, espiresDate);

    var newUserToken = new UserToken(loggedUserId, refreshToken, espiresDate);

    await _userTokenReposiotry.CreateAsync(newUserToken, new CancellationToken());

    var userRoles = _mapper.Map<List<UsersRolesViewModel>>(await _usersRolesRepository.FindByUserId(loggedUserId));
    var userSubscriptions = _mapper.Map<List<SubscriptionViewModel>>(await _subscriptionsrepository.ListSubscriptions([loggedUserId], []));

    var authData = new AuthData
    {
      Id = userToken.UserId.ToString(),
      User = _mapper.Map<UserViewModel>(userToken.User),
    };

    authData.Token = tokenService.Create(_mapper.Map<UserViewModel>(userToken.User), Configuration.Secrets.Token, DateTime.UtcNow.AddHours(2));
    authData.RefreshToken = refreshToken;
    authData.User.Subscriptions = userSubscriptions;
    authData.User.UsersRoles = userRoles;

    return new CommandResult(true, "USER_GOTTEN", authData, null, 200);
  }
}