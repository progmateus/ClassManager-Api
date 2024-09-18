using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListStudentsByClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IMapper _mapper;
  public ListStudentsByClassHandler(
    IClassRepository classRepository,
    IStudentsClassesRepository studentsClassesRepository,
     IMapper mapper
    )
  {
    _classRepository = classRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classId)
  {
    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var students = _mapper.Map<StudentsClassesViewModel>(await _studentsClassesRepository.ListByUserOrClassOrTenantAsync(null, [tenantId], [classId]));

    return new CommandResult(true, "STUDENTS_LISTED", students, null, 200);
  }
}
