using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
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
    var classFound = await _classRepository.FindClassProfile(tenantId, classDayId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    DateTime lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

    var studentscount = _studentsClassesRepository.CountByClassId(classFound.Id);
    var teachersCount = _teacherClassesRepository.CountByClassId(classFound.Id);

    var classesDaysOfTheMonth = _classDayRepository.CountByClassId(classFound.Id, firstDayOfMonth, lastDayOfMonth);

    var response = new
    {
      classFound,
      teachersCount,
      studentscount,
      classesDaysOfTheMonth
    };

    return new CommandResult(true, "CLASS_PROFILE_GOTTEN", response, null, 200);
  }
}
