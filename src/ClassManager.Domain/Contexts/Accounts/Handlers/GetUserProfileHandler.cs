using AutoMapper;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Services;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.ViewModels;
using ClassManager.Shared.Commands;
namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class GetUserProfileHandler
{
  private readonly IUserRepository _userReporitory;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly ISubscriptionRepository _subscriptionsrepository;
  private readonly IMapper _mapper;
  public GetUserProfileHandler(
    IUserRepository userRepository,
    IUsersRolesRepository usersRolesRepository,
    ISubscriptionRepository subscriptionRepository,
    IMapper mapper
    )
  {
    _userReporitory = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionsrepository = subscriptionRepository;
    _mapper = mapper;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    /* var user = _mapper.Map<UserProfileViewModel>(await _userReporitory.GetByIdAsync(id, new CancellationToken())); */

    var users = _mapper.Map<IEnumerable<UserProfileViewModel>>(await _userReporitory.GetAsync(x => x.Id == id, [x => x.StudentsClasses]));

    if (!users.Any())
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var user = users.First();

    var userRoles = _mapper.Map<List<UsersRolesViewModel>>(await _usersRolesRepository.FindByUserId(user.Id));
    var subscriptions = _mapper.Map<List<SubscriptionPreviewViewModel>>(await _subscriptionsrepository.ListSubscriptions([user.Id], []));

    user.UsersRoles = userRoles;
    user.Subscriptions = subscriptions;
    return new CommandResult(true, "USER_GOTTEN", user, null, 200);
  }
}
