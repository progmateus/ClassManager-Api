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

public class AddTeacherToClassHandler
{
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IMapper _mapper;
  private readonly ITeacherClassesRepository _teachersClassesRepository;

  public AddTeacherToClassHandler(
    ITeacherClassesRepository teachersClassesRepository,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository,
    IMapper mapper


    )
  {
    _teachersClassesRepository = teachersClassesRepository;
    _accessControlService = accessControlService;
    _usersRolesRepository = usersRolesRepository;
    _mapper = mapper;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, UserClassCommand command)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var tenantTeacher = await _usersRolesRepository.ListByRoleAsync(tenantId, ["teacher"], [command.UserId]);

    if (tenantTeacher.Count == 0)
    {
      return new CommandResult(false, "ERR_TEACHER_NOT_FOUND", null, null, 404);
    }

    var teacherAlreadyOnClass = await _teachersClassesRepository.FindByUserIdAndClassId(command.ClassId, command.UserId);

    if (teacherAlreadyOnClass is not null)
    {
      return new CommandResult(false, "ERR_TEACHER_ALREADY_ON_CLASS", null, null, 409);
    }

    var teacherClass = new TeachersClasses(command.UserId, command.ClassId);

    await _teachersClassesRepository.CreateAsync(teacherClass, new CancellationToken());

    return new CommandResult(true, "TEACHER_ADDED", _mapper.Map<TeachersClassesViewModel>(teacherClass), null, 200);
  }
}
