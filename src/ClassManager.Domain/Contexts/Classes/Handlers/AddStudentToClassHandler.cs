using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class AddStudentToClassHandler
{
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IMapper _mapper;

  public AddStudentToClassHandler(
    IStudentsClassesRepository studentsClassesRepository,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository,
    IMapper mapper


    )
  {
    _studentsClassesRepository = studentsClassesRepository;
    _accessControlService = accessControlService;
    _usersRolesRepository = usersRolesRepository;
    _mapper = mapper;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UpdateStudentClassCommand command)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenantStudent = await _usersRolesRepository.ListByRoleAsync(tenantId, ["student"], [command.UserId]);

    if (tenantStudent.Count == 0)
    {
      return new CommandResult(false, "ERR_STUDENT_NOT_FOUND", null, null, 404);
    }

    var studentAlreadyOnClass = await _studentsClassesRepository.FindByUserIdAndClassId(command.ClassId, command.UserId);

    if (studentAlreadyOnClass is not null)
    {
      return new CommandResult(false, "ERR_STUDENT_ALREADY_ON_CLASS", null, null, 409);
    }

    var studentClass = new StudentsClasses(command.UserId, command.ClassId);

    await _studentsClassesRepository.CreateAsync(studentClass, new CancellationToken());

    return new CommandResult(true, "STUDENT_ADDED", _mapper.Map<StudentsClassesViewModel>(studentClass), null, 200);
  }
}
