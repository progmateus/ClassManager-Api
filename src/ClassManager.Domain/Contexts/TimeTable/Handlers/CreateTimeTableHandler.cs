using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.TimeTables.Handlers;

public class CreateTimeTableHandler :
  Notifiable, ITenantHandler<CreateTimeTableCommand>
{
  private readonly ITimeTableRepository _timeTableRepository;

  public CreateTimeTableHandler(
    ITimeTableRepository classHourRepository
    )
  {
    _timeTableRepository = classHourRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, CreateTimeTableCommand command)
  {

    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    TimeTable timeTable = new TimeTable(tenantId, command.Name);


    await _timeTableRepository.CreateAsync(timeTable, new CancellationToken());

    return new CommandResult(true, "TIME_TABLE_CREATED", "", null, 201);
  }
}
