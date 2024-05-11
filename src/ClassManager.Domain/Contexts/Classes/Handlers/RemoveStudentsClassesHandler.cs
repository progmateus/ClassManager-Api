using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class RemoveStudentsClassesHandler
{
  private readonly ITenantRepository _tenantRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;

  public RemoveStudentsClassesHandler(
    ITenantRepository tenantRepository,
    IStudentsClassesRepository teachersClassesRepository
    )
  {
    _tenantRepository = tenantRepository;
    _studentsClassesRepository = teachersClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid teacherClassId)
  {
    var tenant = await _tenantRepository.IdExistsAsync(tenantId, new CancellationToken());
    if (!tenant)
    {
      return new CommandResult(false, "ERR_TENANT_NOT_FOUND", null, null, 404);
    }

    var studentClass = await _studentsClassesRepository.GetByIdAsync(teacherClassId, new CancellationToken());

    if (studentClass is null)
    {
      return new CommandResult(false, "ERR_STUDENT_NOT_FOUND", null, null, 404);
    }

    await _studentsClassesRepository.DeleteAsync(studentClass.Id, new CancellationToken());

    return new CommandResult(true, "STUDENT_REMOVED", studentClass, null, 204);
  }
}
