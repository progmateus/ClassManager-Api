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

public class CreateUserRoleHandler : Notifiable,
  ITenantHandler<CreateUserRoleCommand>
{
  private IRoleRepository _roleRepository;
  private IUserRepository _userRepository;
  private IUsersRolesRepository _usersRolesRepository;
  private IAccessControlService _accessControlService;
  public CreateUserRoleHandler(IRoleRepository roleRepository, IUserRepository userRepository, IUsersRolesRepository usersRolesRepository, IAccessControlService accessControlService)
  {
    _roleRepository = roleRepository;
    _userRepository = userRepository;
    _usersRolesRepository = usersRolesRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateUserRoleCommand command)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var roleFound = await _roleRepository.GetByNameAsync(command.RoleName, new CancellationToken());

    if (roleFound is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_FOUND", null, null, 404);
    }

    if (!await _userRepository.IdExistsAsync(command.UserId, new CancellationToken()))
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var userRoleAlreadyExists = await _usersRolesRepository.HasAnyRoleAsync(command.UserId, tenantId, [command.RoleName], new CancellationToken());

    if (userRoleAlreadyExists)
    {
      return new CommandResult(false, "ERR_USER_ROLE_ALREADY_EXISTS", null, null, 404);
    }

    var userRole = new UsersRoles(command.UserId, roleFound.Id, tenantId);

    await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());

    var userRoleCreated = await _usersRolesRepository.FindByIdWithInclude(userRole.Id, tenantId);

    return new CommandResult(false, "ROLE_CREATED", userRoleCreated, null, 201);
  }
}