using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
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
  public GetUserProfileHandler(
    IUserRepository userRepository,
    IUsersRolesRepository usersRolesRepository,
    ISubscriptionRepository subscriptionRepository
    )
  {
    _userReporitory = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _subscriptionsrepository = subscriptionRepository;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    var user = await _userReporitory.GetByIdAsync(id, new CancellationToken());

    if (user == null)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var userRoles = await _usersRolesRepository.FindByUserId(user.Id);
    var subscriptions = await _subscriptionsrepository.ListSubscriptions(user.Id, null);

    /*     var userModel = new UserViewModel
        {
          Id = user.Id,
          FirstName = user.Name.FirstName,
          LastName = user.Name.LastName,
          Email = user.Email.ToString(),
          Document = user.Document.ToString(),
          Avatar = user.Avatar,
          Username = user.Username,
          Status = user.Status,
          Type = user.Type,
          Subscriptions = subscriptions,
          UsersRoles = userRoles ?? [],
          CreatedAt = user.CreatedAt,
          UpdatedAt = user.UpdatedAt,
        }; */

    return new CommandResult(true, "USER_GOTTEN", user, null, 200);
  }
}
