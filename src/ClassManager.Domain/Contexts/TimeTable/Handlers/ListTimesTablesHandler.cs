using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.TimesTabless.Handlers;

public class ListTimesTablesHandler
{
  private readonly ITimeTableRepository _timeTableRepository;
  private IAccessControlService _accessControlService;

  public ListTimesTablesHandler(
    ITimeTableRepository classHourRepository,
    IAccessControlService accessControlService
    )
  {
    _timeTableRepository = classHourRepository;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId)
  {

    if (!await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId, ["admin"]))
    {
      return new CommandResult(false, "ERR_ADMIN_ROLE_NOT_FOUND", null, null, 403);
    }

    var timeTable = await _timeTableRepository.ListByTenantId(tenantId, new CancellationToken());

    return new CommandResult(true, "TIMES_TABLES_LISTED", timeTable, null, 200);
  }
}
