using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class AddStudentsClassesHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IUserRepository _userRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;

  public AddStudentsClassesHandler(
    IClassRepository classRepository,
    IUserRepository userRepository,
    IStudentsClassesRepository studentsClassesRepository
    )
  {
    _classRepository = classRepository;
    _userRepository = userRepository;
    _studentsClassesRepository = studentsClassesRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, CreateUserClassCommand command)
  {
    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, command.ClassId, new CancellationToken());
    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var user = await _userRepository.IdExistsAsync(command.UserId, default);

    if (!user)
    {
      return new CommandResult(false, "ERR_USER_NOT_FOUND", null, null, 404);
    }

    var teacherclassalreadyExists = await _studentsClassesRepository.GetByUserIdAndClassId(command.ClassId, command.UserId);

    if (!(teacherclassalreadyExists is null))
    {
      return new CommandResult(false, "STUDENT_ALREADY_ADDED", null, null, 409);
    }

    var studentClass = new StudentsClasses(command.UserId, command.ClassId);

    await _studentsClassesRepository.CreateAsync(studentClass, new CancellationToken());

    return new CommandResult(true, "STUDENT_ADDED", studentClass, null, 200);
  }
}
