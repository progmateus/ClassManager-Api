using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class RemoveTeachersClassesHandler
{
  private readonly IClassRepository _classRepository;
  private readonly ITenantRepository _tenantRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;

  public RemoveTeachersClassesHandler(
    IClassRepository classRepository,
    ITenantRepository tenantRepository,
    ITeacherClassesRepository teachersClassesRepository
    )
  {
    _classRepository = classRepository;
    _tenantRepository = tenantRepository;
    _teachersClassesRepository = teachersClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid teacherClassId)
  {
    var tenant = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());
    if (!tenant)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var teacherClass = await _teachersClassesRepository.GetByIdAsync(teacherClassId, new CancellationToken());

    if (teacherClass is null)
    {
      return new CommandResult(false, "ERR_TACHER_NOT_FOUND", null, null, 404);
    }

    await _teachersClassesRepository.DeleteAsync(teacherClass.Id, new CancellationToken());

    return new CommandResult(true, "TEACHER_REMOVED", teacherClass, null, 204);
  }
}
