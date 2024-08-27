using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListTeachersByClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  public ListTeachersByClassHandler(
    IClassRepository classRepository,
    ITeacherClassesRepository teachersClassesRepository
    )
  {
    _classRepository = classRepository;
    _teachersClassesRepository = teachersClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classId)
  {
    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var teachers = await _teachersClassesRepository.ListByClassId(classId);

    return new CommandResult(true, "TEACHERS_LISTED", teachers, null, 200);
  }
}
