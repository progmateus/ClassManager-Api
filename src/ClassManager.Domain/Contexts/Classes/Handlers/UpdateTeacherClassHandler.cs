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
  private readonly IClassRepository _classRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;


  public UpdateTeacherClassHandler(
    IClassRepository classRepository,
    ITeacherClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository

    )
  {
    _classRepository = classRepository;
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

    var classEntity = await _classRepository.GetByIdAndTenantIdAsync(tenantId, command.ClassId, new CancellationToken());

    if (classEntity is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var tenantTeachersfound = await _usersRolesRepository.ListByRoleAsync(tenantId, ["teacher"], command.UsersIds);

    var userAlreadyOnClass = await _teachersClassesRepository.GetByUsersIdsAndClassesIds(tenantId, command.UsersIds, [classEntity.Id]);

    var newTeachersclass = new List<TeachersClasses>();

    await _teachersClassesRepository.DeleteRangeAsync(userAlreadyOnClass, new CancellationToken());

    foreach (var tenantTeacher in tenantTeachersfound)
    {
      newTeachersclass.Add(new TeachersClasses(tenantTeacher.UserId, classEntity.Id));
    }

    await _teachersClassesRepository.CreateRangeAsync(newTeachersclass, new CancellationToken());

    return new CommandResult(true, "TEACHERS_ADDED", newTeachersclass, null, 200);
  }
}
