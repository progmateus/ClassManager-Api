using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListStudentsByClassHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public ListStudentsByClassHandler(
    IClassRepository classRepository,
    IStudentsClassesRepository studentsClassesRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _classRepository = classRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classId, PaginationCommand command)
  {
    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin", "student"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var classFound = await _classRepository.GetByIdAndTenantIdAsync(tenantId, classId, new CancellationToken());

    if (classFound is null)
    {
      return new CommandResult(false, "ERR_CLASS_NOT_FOUND", null, null, 404);
    }

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var students = _mapper.Map<List<StudentsClassesViewModel>>(await _studentsClassesRepository.ListByUserOrClassAndTenantAsync([], [tenantId], [classId], command.Search, skip, command.Limit));

    return new CommandResult(true, "STUDENTS_LISTED", students, null, 200);
  }
}
