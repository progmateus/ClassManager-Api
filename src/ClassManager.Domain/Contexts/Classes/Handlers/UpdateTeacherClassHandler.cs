using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateTeacherClassHandler
{
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;


  public UpdateTeacherClassHandler(
    ITeacherClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository

    )
  {
    _teachersClassesRepository = teachersClassesRepository;
    _accessControlService = accessControlService;
    _usersRolesRepository = usersRolesRepository;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UpdateUserClassCommand command)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    await _teachersClassesRepository.DeleteByUsersAndClasses(tenantId, [command.ClassId], command.UsersIds, new CancellationToken());

    if (command.UsersIds.Count == 0)
    {
      return new CommandResult(true, "TEACHERS_ADDED", new { }, null, 200);
    }

    var tenantTeachersfound = await _usersRolesRepository.ListByRoleAsync(tenantId, ["teacher"], command.UsersIds);

    var newTeachersclass = new List<TeachersClasses>();

    foreach (var tenantTeacher in tenantTeachersfound)
    {
      newTeachersclass.Add(new TeachersClasses(tenantTeacher.UserId, command.ClassId));
    }

    await _teachersClassesRepository.CreateRangeAsync(newTeachersclass, new CancellationToken());

    return new CommandResult(true, "TEACHERS_ADDED", new { }, null, 200);
  }
}
