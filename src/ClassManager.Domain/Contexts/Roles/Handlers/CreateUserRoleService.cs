using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class CreateUserRoleHandler : Notifiable
{
  private IRoleRepository _roleRepository;
  private IUserRepository _userRepository;
  private ITenantRepository _tenantRepository;
  private IUsersRolesRepository _usersRolesRepository;
  public CreateUserRoleHandler(IRoleRepository roleRepository, IUserRepository userRepository, ITenantRepository tenantRepository, IUsersRolesRepository usersRolesRepository)
  {
    _roleRepository = roleRepository;
    _userRepository = userRepository;
    _tenantRepository = tenantRepository;
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid userId, string roleName)
  {
    var roleFound = await _roleRepository.GetByNameAsync(roleName, new CancellationToken());

    if (roleFound is null)
    {
      return new CommandResult(false, "ERR_ROLE_NOT_EXISTS", null, null, 404);
    }

    var tenantExists = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());

    if (!tenantExists)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_EXISTS", null, null, 404);
    }

    var userExists = await _userRepository.IdExistsAsync(userId, new CancellationToken());

    if (!userExists)
    {
      return new CommandResult(false, "ERR_USER_NOT_EXISTS", null, null, 404);
    }

    var userRoleAlreadyExists = await _usersRolesRepository.VerifyRoleExistsAsync(userId, tenantId, roleName, new CancellationToken());


    if (userRoleAlreadyExists)
    {
      return new CommandResult(false, "ERR_USER_ROLE_ALREADY_EXISTS", null, null, 404);
    }

    var userRole = new UsersRoles(userId, roleFound.Id, tenantId);

    await _usersRolesRepository.CreateAsync(userRole, new CancellationToken());

    return new CommandResult(false, "ROLE_CREATED", null, null, 201);
  }
}