using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class GetClassProfileHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly ITeacherClassesRepository _teacherClassesRepository;
  private readonly IClassDayRepository _classDayRepository;
  public GetClassProfileHandler(
    IClassRepository classRepository,
    IStudentsClassesRepository studentsClassesRepository,
    ITeacherClassesRepository teacherClassesRepository,
    IClassDayRepository classDayRepository

    )
  {
    _classRepository = classRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _teacherClassesRepository = teacherClassesRepository;
    _classDayRepository = classDayRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classDayId)
  {
    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classDayId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOOT_FOUND", null, null, 404);
    }

    var studentscount = _studentsClassesRepository.CountByClassId(classFound.Id);
    var teahcersCount = _teacherClassesRepository.CountByClassId(classFound.Id);

    var classesDays = _classDayRepository.CountByClassId(classFound.Id);

    var teste = new
    {
      classFound,
      teahcersCount,
      studentscount,
      classesDays
    };

    return new CommandResult(true, "CLASS_DAY_GOTTEN", teste, null, 200);
  }
}
