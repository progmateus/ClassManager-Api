using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.TimeTables.Handlers;

public class GetTimeTableHandler
{
  private readonly ITimeTableRepository _timeTableRepository;
  private IAccessControlService _accessControlService;

  public GetTimeTableHandler(
    ITimeTableRepository classHourRepository,
    IAccessControlService accessControlService
    )
  {
    _timeTableRepository = classHourRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid entityId)
  {

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTable = await _timeTableRepository.FindByIdAndTenantIdAsync(entityId, tenantId, new CancellationToken());

    return new CommandResult(true, "TIME_TABLE_GOTTEN", timeTable, null, 200);
  }
}
