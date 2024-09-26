using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class GetUserRolesHandler : Notifiable,
  ITenantHandler<UsersRolesCommand>
{
  private IUserRepository _userRepository;
  private ITenantRepository _tenantRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private IAccessControlService _accessControlService;
  public GetUserRolesHandler(IUserRepository userRepository, ITenantRepository tenantRepository, IUsersRolesRepository usersRolesRepository, IAccessControlService accessControlService)
  {
    _userRepository = userRepository;
    _tenantRepository = tenantRepository;
    _usersRolesRepository = usersRolesRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UsersRolesCommand command)
  {
    command.Validate();

    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_USER_ROLE_NOT_LISTED", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var userExists = await _userRepository.IdExistsAsync(command.UserId, new CancellationToken());

    if (!userExists)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var usersRoles = await _usersRolesRepository.ListUsersRolesByUserIdAndTenantId(command.UserId, tenantId, new CancellationToken());

    return new CommandResult(false, "USER_ROLES_LISTED", usersRoles, null, 200);
  }
}