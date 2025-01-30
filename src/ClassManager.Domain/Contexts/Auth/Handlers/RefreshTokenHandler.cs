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
  public async Task<ICommandResult> Handle(RefreshTokenCommand command)
  {

    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_INVALID_REFRESH_TOKEN", null, null, 401);
    }

    var tokenService = new TokenService();

    if (tokenService.ValidateToken(command.RefreshToken))
    {
      return new CommandResult(false, "ERR_INVALID_REFRESH_TOKEN", null, null, 401);
    }

    var userToken = await _userTokenReposiotry.FindByRefreshToken(command.RefreshToken, default);

    if (userToken is null)
    {
      return new CommandResult(false, "ERR_INVALID_REFRESH_TOKEN", null, null, 401);
    }


    var userClaims = tokenService.GetJwtClaims(command.RefreshToken);

    var userViewModel = _mapper.Map<UserViewModel>(userToken.User);


    var expiresAt = DateTime.UtcNow.AddHours(30);

    var refreshToken = tokenService.Create(userViewModel, Configuration.Secrets.RefreshToken, expiresAt);

    var newUserToken = new UserToken(new Guid(userClaims.Id), refreshToken, expiresAt);

    await _userTokenReposiotry.CreateAsync(newUserToken, new CancellationToken());

    var userRoles = _mapper.Map<List<UsersRolesViewModel>>(await _usersRolesRepository.FindByUserId(new Guid(userClaims.Id)));
    var userSubscriptions = _mapper.Map<List<SubscriptionViewModel>>(await _subscriptionsrepository.ListSubscriptions([new Guid(userClaims.Id)], []));

    var authData = new AuthData
    {
      Id = userToken.UserId.ToString(),
      User = userViewModel,
    };

    authData.Token = tokenService.Create(userViewModel, Configuration.Secrets.Token, DateTime.UtcNow.AddHours(2));
    authData.RefreshToken = refreshToken;
    authData.User.Subscriptions = userSubscriptions;
    authData.User.UsersRoles = userRoles;

    return new CommandResult(true, "USER_GOTTEN", authData, null, 200);
  }
}