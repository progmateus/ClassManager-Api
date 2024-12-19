using ClassManager.Domain.Contexts.Accounts.Commands;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;
using MassTransit.Initializers;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class TransferStudentsClassHandler :
  Notifiable,
  ITenantActionHandler<TransferStudentsClassCommand>
{
  private readonly IAccessControlService _accessControlService;
  private readonly IStudentsClassesRepository _studentsClassesRepository;

  public TransferStudentsClassHandler(
    IAccessControlService accessControlService,
    IStudentsClassesRepository studentsClassesRepository
    )
  {
    _accessControlService = accessControlService;
    _studentsClassesRepository = studentsClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, TransferStudentsClassCommand command)
  {
    // fail fast validation
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classFromStudents = await _studentsClassesRepository.GetByUsersIdsAndClassesIds(tenantId, [], [classId]);
    var classToStudents = await _studentsClassesRepository.GetByUsersIdsAndClassesIds(tenantId, [], [command.ToId]);

    var classFromStudentsIds = classFromStudents.Select(x => x.UserId);
    var classToStudentsIds = classToStudents.Select(x => x.UserId);

    var allStudentsIds = classFromStudentsIds.Where(cf => !classToStudentsIds.Contains(cf));
    var newStudents = new List<StudentsClasses>();


    foreach (var studentId in allStudentsIds)
    {
      var studentClass = new StudentsClasses(studentId, command.ToId);
      newStudents.Add(studentClass);

    }
    await _studentsClassesRepository.CreateRangeAsync(newStudents, new CancellationToken());
    await _studentsClassesRepository.DeleteByClassId(tenantId, classId, new CancellationToken());

    return new CommandResult(true, "STUDENS_TRANFERED", new { }, null, 201);
  }
}
