using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class RemoveTeacherFromClassHandler
{
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  private readonly IAccessControlService _accessControlService;


  public RemoveTeacherFromClassHandler(
    ITeacherClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService

    )
  {
    _teachersClassesRepository = teachersClassesRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid teacherClassId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId, "admin"))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var teacherClass = await _teachersClassesRepository.GetByIdAsync(teacherClassId, new CancellationToken());

    if (teacherClass is null)
    {
      return new CommandResult(false, "ERR_TACHER_NOT_FOUND", null, null, 404);
    }

    await _teachersClassesRepository.DeleteAsync(teacherClassId, new CancellationToken());

    return new CommandResult(true, "TEACHER_REMOVED", teacherClass, null, 204);
  }
}
