using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersByRoleHandler
{
  private readonly IUsersRolesRepository _usersRolesRepository;
  public ListUsersByRoleHandler(
    IUsersRolesRepository userRepository
    )
  {
    _usersRolesRepository = userRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, string roleName)
  {
    var users = await _usersRolesRepository.ListByRoleAsync(tenantId, roleName);

    return new CommandResult(true, "USERS_ROLES_LISTED", users, null, 200);
  }
}
