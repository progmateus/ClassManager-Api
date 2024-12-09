using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateStudentClassHandler
{
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;

  public UpdateStudentClassHandler(
    IStudentsClassesRepository studentsClassesRepository,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository


    )
  {
    _studentsClassesRepository = studentsClassesRepository;
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

    await _studentsClassesRepository.DeleteByUserIdAndtenantId(tenantId, command.UsersIds, new CancellationToken());

    if (command.UsersIds.Count == 0)
    {
      return new CommandResult(true, "STUDENT_ADDED", new { }, null, 200);
    }

    var tenantStudents = await _usersRolesRepository.ListByRoleAsync(tenantId, ["student"], command.UsersIds);

    var newStudentsClass = new List<StudentsClasses>();

    foreach (var tenantStudent in tenantStudents)
    {
      newStudentsClass.Add(new StudentsClasses(tenantStudent.UserId, command.ClassId));
    }

    await _studentsClassesRepository.CreateRangeAsync(newStudentsClass, new CancellationToken());

    return new CommandResult(true, "STUDENT_ADDED", new { }, null, 200);
  }
}
