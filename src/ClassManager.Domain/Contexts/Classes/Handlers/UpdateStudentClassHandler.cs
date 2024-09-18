using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateStudentClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IUserRepository _userRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;

  public UpdateStudentClassHandler(
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

    var student_class = await _studentsClassesRepository.FindByUserIdAndClassId(command.ClassId, command.UserId);

    if (student_class is not null)
    {
      return new CommandResult(false, "STUDENT_ALREADY_ADDED", null, null, 409);
    }

    var userAlreadyOnClass = await _studentsClassesRepository.ListByUserOrClassOrTenantAsync([command.UserId], [tenantId], null);

    await _studentsClassesRepository.DeleteRangeAsync(userAlreadyOnClass, new CancellationToken());

    var studentClass = new StudentsClasses(command.UserId, command.ClassId);

    await _studentsClassesRepository.CreateAsync(studentClass, new CancellationToken());

    return new CommandResult(true, "STUDENT_ADDED", studentClass, null, 200);
  }
}
