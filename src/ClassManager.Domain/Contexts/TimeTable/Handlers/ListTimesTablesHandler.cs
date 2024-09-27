using AutoMapper;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.TimesTabless.Handlers;

public class ListTimesTablesHandler
{
  private readonly ITimeTableRepository _timeTableRepository;
  private IAccessControlService _accessControlService;
  private IMapper _mapper;

  public ListTimesTablesHandler(
    ITimeTableRepository classHourRepository,
    IAccessControlService accessControlService,
    IMapper mapper
    )
  {
    _timeTableRepository = classHourRepository;
    _accessControlService = accessControlService;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTables = _mapper.Map<List<TimeTableViewModel>>(await _timeTableRepository.ListByTenantId(tenantId, new CancellationToken())); ;

    return new CommandResult(true, "TIMES_TABLES_LISTED", timeTables, null, 200);
  }
}
