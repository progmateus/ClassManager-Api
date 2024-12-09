using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListTeacherClassesHandler
{
  private readonly ITeacherClassesRepository _teacherClassesRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public ListTeacherClassesHandler(
    ITeacherClassesRepository teacherClassesRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _teacherClassesRepository = teacherClassesRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid targetUserId)
  {
    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var teacherClasses = _mapper.Map<List<TeachersClassesViewModel>>(await _teacherClassesRepository.ListByUserOrClassOrTenantAsync([targetUserId], [tenantId], []));

    return new CommandResult(true, "TEACHER_CLASSES_LISTED", teacherClasses, null, 200);
  }
}
