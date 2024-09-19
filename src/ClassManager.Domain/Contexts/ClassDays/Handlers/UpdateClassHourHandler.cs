using ClassManager.Domain.Contexts.ClassDays.Commands;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using Flunt.Notifications;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class UpdateClassHourHandler :
  Notifiable
{
  private readonly IClassHourRepository _classHourRepository;

  public UpdateClassHourHandler(
    IClassHourRepository classHourRepository
    )
  {
    _classHourRepository = classHourRepository;
  }
  public async Task<ICommandResult> Handle(UpdateClassHourCommand command, Guid tenantId)
  {
    if (command.ClassesHours.IsNullOrEmpty())
    {
      return new CommandResult(false, "ERR_INVALID_CLASS_HOUR", null, null, 400);
    }

    var grouped = command.ClassesHours
    .GroupBy(ch => ch.WeekDay)
    .Select(grp => grp.ToList())
    .ToList();

    List<ClassHour> classesHoursEntities = [];

    foreach (var classHour in command.ClassesHours)
    {
      var classHourEntity = new ClassHour(classHour.WeekDay, classHour.HourStart, classHour.HourEnd, tenantId);
      classesHoursEntities.Add(classHourEntity);
    }

    await _classHourRepository.DeleteAllByTenantIdAsync(tenantId, new CancellationToken());

    await _classHourRepository.CreateRangeAsync(classesHoursEntities, new CancellationToken());

    return new CommandResult(true, "CLASS_HOUR_CREATED", "", null, 201);
  }
}
