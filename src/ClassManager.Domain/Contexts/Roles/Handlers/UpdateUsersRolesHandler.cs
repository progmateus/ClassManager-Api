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

public class UpdateUsersRolesHandler : Notifiable,
  IHandler<UsersRolesCommand>
{
  private IRoleRepository _roleRpository;
  private IUserRepository _userRepository;
  private ITenantRepository _tenantRepository;
  private IUsersRolesRepository _usersRolesRepository;

  public UpdateUsersRolesHandler(IRoleRepository roleRepository, IUserRepository userRepository, ITenantRepository tenantRepository, IUsersRolesRepository usersRolesRepository)
  {
    _roleRpository = roleRepository;
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
      return new CommandResult(false, "ERR_USER_ROLE_NOT_ADDED", null, command.Notifications);
    }

    var roleExists = await _roleRpository.GetByIdsAsync(command.RolesIds, new CancellationToken());

    if (roleExists.Count != command.RolesIds.Count)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_EXISTS", null, null, 404);
    }

    var tenantExists = await _tenantRepository.IdExistsAsync(command.TenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_EXISTS", null, null, 404);
    }

    var userExists = await _userRepository.IdExistsAsync(command.UserId, new CancellationToken());

    if (!userExists)
    {
      return new CommandResult(false, "ERR_USER_NOT_EXISTS", null, null, 404);
    }

    List<UsersRoles> usersRoles = [];
    foreach (Guid roleId in command.RolesIds)
    {
      var userRole = new UsersRoles(command.UserId, roleId, command.TenantId);
      usersRoles.Add(userRole);
    }
    await _usersRolesRepository.DeleteByUserIdAndtenantId(command.UserId, command.TenantId, new CancellationToken());

    await _usersRolesRepository.CreateRangeAsync(usersRoles, new CancellationToken());

    return new CommandResult(false, "USER_ROLES_ADDED", null, null, 201);
  }
}