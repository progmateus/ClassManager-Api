using AutoMapper;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class GetClassDayByIdHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  public GetClassDayByIdHandler(
    IClassDayRepository classRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _classDayRepository = classRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid classDayId)
  {

    if (!await _accessControlService.IsTenantSubscriptionActiveAsync(tenantId))
    {
      return new CommandResult(false, "ERR_TENANT_INACTIVE", null, null, 403);
    }

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin", "student", "teacher"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    var classDay = await _classDayRepository.FindClassDayProfile(tenantId, classDayId);

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    if (!await _accessControlService.HasClassRoleAsync(loggedUserId, tenantId, classDay.ClassId, ["student", "teacher"]))
    {
      return new CommandResult(false, "ERR_PERMISSION_DENIED", null, null, 403);
    }

    return new CommandResult(true, "CLASS_DAY_GOTTEN", _mapper.Map<ClassDayViewModel>(classDay), null, 200);
  }
}
