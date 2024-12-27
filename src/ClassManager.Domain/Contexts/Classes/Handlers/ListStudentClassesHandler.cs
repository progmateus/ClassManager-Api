using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListStudentClassesHandler
{
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;

  public ListStudentClassesHandler(
    IStudentsClassesRepository studentsClassesRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _studentsClassesRepository = studentsClassesRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid targetUserId)
  {

    var targetId = loggedUserId;

    if (await _accessControlService.CheckParameterUserIdPermission(tenantId, loggedUserId, targetUserId))
    {
      targetId = targetUserId;
    }

    var studentClasses = _mapper.Map<List<StudentsClassesViewModel>>(await _studentsClassesRepository.ListByUserOrClassAndTenantAsync([targetId], [tenantId], []));

    return new CommandResult(true, "STUDENT_CLASSES_LISTED", studentClasses, null, 200);
  }
}
