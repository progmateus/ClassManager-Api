using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateTeacherClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IUserRepository _userRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  private readonly IAccessControlService _accessControlService;


  public UpdateTeacherClassHandler(
    IClassRepository classRepository,
    IUserRepository userRepository,
    ITeacherClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService

    )
  {
    _classRepository = classRepository;
    _userRepository = userRepository;
    _teachersClassesRepository = teachersClassesRepository;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, CreateUserClassCommand command)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }


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

    var teacherclassalreadyExists = await _teachersClassesRepository.GetByUserIdAndClassId(command.ClassId, command.UserId);

    if (teacherclassalreadyExists is not null)
    {
      return new CommandResult(false, "TEACHER_ALREADY_ADDED", null, null, 409);
    }

    var userAlreadyOnClass = await _teachersClassesRepository.GetByUserIdAndTenantId(tenantId, command.UserId);

    await _teachersClassesRepository.DeleteRangeAsync(userAlreadyOnClass, new CancellationToken());

    var teacherClass = new TeachersClasses(command.UserId, command.ClassId);

    await _teachersClassesRepository.CreateAsync(teacherClass, new CancellationToken());

    return new CommandResult(true, "TEACHER_ADDED", teacherClass, null, 200);
  }
}
