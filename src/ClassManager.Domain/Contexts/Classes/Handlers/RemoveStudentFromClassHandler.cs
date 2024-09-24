using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class RemoveStudentFromClassHandler : ITenantDeleteActionHandler
{
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IAccessControlService _accessControlService;


  public RemoveStudentFromClassHandler(
    ITenantRepository tenantRepository,
    IStudentsClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService

    )
  {
    _studentsClassesRepository = teachersClassesRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid studentClassId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin"))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var studentClass = await _studentsClassesRepository.GetByIdAsync(studentClassId, new CancellationToken());

    if (studentClass is null)
    {
      return new CommandResult(false, "ERR_STUDENT_NOT_FOUND", null, null, 404);
    }

    await _studentsClassesRepository.DeleteAsync(studentClass.Id, new CancellationToken());

    return new CommandResult(true, "STUDENT_REMOVED", studentClass, null, 204);
  }
}
