using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class UpdateClassDayHandler :
  Notifiable,
  IActionHandler<UpdateClassDayCommand>
{
  private readonly IClassDayRepository _classDayRepository;

  public UpdateClassDayHandler(
    IClassDayRepository classDayRepository
    )
  {
    _classDayRepository = classDayRepository;
  }
  public async Task<ICommandResult> Handle(Guid classDayId, UpdateClassDayCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_UPDATED", null, command.Notifications);
    }

    var classDay = await _classDayRepository.GetByIdAsync(classDayId, new CancellationToken());

    if (classDay is null)
    {
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_FOUND", null, null, 404);
    }

    classDay.ChangeStatus(command.Status, command.Observation);

    await _classDayRepository.UpdateAsync(classDay, new CancellationToken());

    return new CommandResult(true, "CLASS_DAY_UPDATED", classDay, null, 201);
  }
}
