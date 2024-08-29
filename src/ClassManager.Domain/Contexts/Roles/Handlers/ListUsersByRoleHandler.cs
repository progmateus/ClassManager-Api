using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Accounts.Handlers;

public class ListUsersRolesHandler
{
  private readonly IUsersRolesRepository _usersRolesRepository;
  public ListUsersRolesHandler(
    IUsersRolesRepository userRepository
    )
  {
    _usersRolesRepository = userRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, List<string> rolesNames, List<Guid> usersIds)
  {
    var users = await _usersRolesRepository.ListByRoleAsync(tenantId, rolesNames, usersIds);

    return new CommandResult(true, "USERS_ROLES_LISTED", users, null, 200);
  }
}
