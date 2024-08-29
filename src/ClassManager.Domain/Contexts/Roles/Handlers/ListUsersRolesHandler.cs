using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class GetUserRolesHandler : Notifiable,
  IHandler<UsersRolesCommand>
{
  private IUserRepository _userRepository;
  private ITenantRepository _tenantRepository;
  private IUsersRolesRepository _usersRolesRepository;
  public GetUserRolesHandler(IUserRepository userRepository, ITenantRepository tenantRepository, IUsersRolesRepository usersRolesRepository)
  {
    _userRepository = userRepository;
    _tenantRepository = tenantRepository;
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(UsersRolesCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_USER_ROLE_NOT_LISTED", null, command.Notifications);
    }

    var tenantExists = await _tenantRepository.IdExistsAsync(command.TenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var userExists = await _userRepository.IdExistsAsync(command.UserId, new CancellationToken());

    if (!userExists)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var usersRoles = await _usersRolesRepository.ListUsersRolesByUserIdAndTenantId(command.UserId, command.TenantId, new CancellationToken());

    return new CommandResult(false, "USER_ROLES_LISTED", usersRoles, null, 200);
  }
}