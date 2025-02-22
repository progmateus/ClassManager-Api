using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListTeachersByClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly ITeacherClassesRepository _teachersClassesRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public ListTeachersByClassHandler(
    IClassRepository classRepository,
    ITeacherClassesRepository teachersClassesRepository,
    IMapper mapper,
    IAccessControlService accessControlService

    )
  {
    _classRepository = classRepository;
    _teachersClassesRepository = teachersClassesRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;

  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, PaginationCommand command)
  {

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin", "student"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    var teachers = _mapper.Map<List<TeachersClassesViewModel>>(await _teachersClassesRepository.ListByUserOrClassOrTenantAsync([], [tenantId], [classId], command.Search, skip, command.Limit));

    return new CommandResult(true, "TEACHERS_LISTED", teachers, null, 200);
  }
}
