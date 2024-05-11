using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class AddTeachersClassesandle
{
  private readonly IClassRepository _classRepository;
  private readonly IUserRepository _userRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;

  public AddTeachersClassesandle(
    IClassRepository classRepository,
    IUserRepository userRepository,
    ITeacherClassesRepository teachersClassesRepository
    )
  {
    _classRepository = classRepository;
    _userRepository = userRepository;
    _teachersClassesRepository = teachersClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, CreateUserClassCommand command)
  {
    var classFound = await _classRepository.GetByIdAndTenantId(tenantId, command.ClassId, new CancellationToken());
    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var user = await _userRepository.IdExistsAsync(command.UserId, default);

    if (!user)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var teacherclassalreadyExists = await _teachersClassesRepository.GetByUserIdAndClassId(command.ClassId, command.UserId);

    if (!(teacherclassalreadyExists is null))
    {
      return new CommandResult(false, "TEACHER_ALREADY_ADDED", null, null, 409);
    }

    var teacherClass = new TeachersClasses(command.UserId, command.ClassId);

    await _teachersClassesRepository.CreateAsync(teacherClass, new CancellationToken());

    return new CommandResult(true, "TEACHER_ADDED", teacherClass, null, 200);
  }
}
