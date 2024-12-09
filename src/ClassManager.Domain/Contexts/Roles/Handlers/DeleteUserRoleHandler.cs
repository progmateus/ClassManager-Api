using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Roles.Handlers;

public class DeleteUserRoleHandler : Notifiable
{
  private IUsersRolesRepository _usersRolesRepository;
  private IAccessControlService _accessControlService;
  private ITeacherClassesRepository _teacherClassesRepository;
  public DeleteUserRoleHandler(IUsersRolesRepository usersRolesRepository, IAccessControlService accessControlService, ITeacherClassesRepository teacherClassesRepository)
  {
    _usersRolesRepository = usersRolesRepository;
    _accessControlService = accessControlService;
    _teacherClassesRepository = teacherClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid userRoleId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var userRole = await _usersRolesRepository.FindByIdAndTenantIdAsync(userRoleId, tenantId, new CancellationToken());

    if (userRole is null)
    {
      return new CommandResult(false, "ERR_USER_ROLE_NOT_FOUND", null, null, 404);
    }

    await _teacherClassesRepository.DeleteByUsersAndClasses(tenantId, [], [userRole.UserId], new CancellationToken());

    return new CommandResult(false, "USER_ROLE_DELETED", null, null, 204);
  }
}