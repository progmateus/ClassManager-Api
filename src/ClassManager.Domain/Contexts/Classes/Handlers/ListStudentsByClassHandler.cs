using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListStudentsByClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  public ListStudentsByClassHandler(
    IClassRepository classRepository,
    IStudentsClassesRepository studentsClassesRepository
    )
  {
    _classRepository = classRepository;
    _studentsClassesRepository = studentsClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classId)
  {
    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var students = await _studentsClassesRepository.ListByClassId(classId, tenantId);

    return new CommandResult(true, "STUDENTS_LISTED", students, null, 200);
  }
}
