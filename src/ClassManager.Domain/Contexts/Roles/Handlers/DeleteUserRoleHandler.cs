using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class DeleteUserRoleHandler : Notifiable
{
  private IUsersRolesRepository _usersRolesRepository;
  public DeleteUserRoleHandler(IUsersRolesRepository usersRolesRepository)
  {
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid id)
  {
    var userRole = await _usersRolesRepository.GetByIdAsync(id, tenantId, new CancellationToken());

    if (userRole is null)
    {
      return new CommandResult(false, "ERR_USER_ROLE_NOT_FOUND", null, null, 404);
    }

    await _usersRolesRepository.DeleteAsync(userRole.Id, tenantId, new CancellationToken());

    return new CommandResult(false, "USER_ROLE_DELETED", null, null, 204);
  }
}