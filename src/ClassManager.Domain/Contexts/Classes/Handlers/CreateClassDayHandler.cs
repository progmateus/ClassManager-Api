using ClassManager.Domain.Contexts.Classes.Commands;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class CreateClassDayHandler :
  Notifiable,
  IHandler<CreateClassDayCommand>
{
  private readonly IClassDayRepository _classDayRepository;

  public CreateClassDayHandler(
    IClassDayRepository classDayRepository
    )
  {
    _classDayRepository = classDayRepository;
  }
  public async Task<ICommandResult> Handle(CreateClassDayCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_CLASS_DAY_NOT_CREATED", null, command.Notifications);
    }

    var classDay = new ClassDay(command.Date, command.HourStart, command.HourEnd, command.ClassId);

    await _classDayRepository.CreateAsync(classDay, new CancellationToken());

    return new CommandResult(true, "CLASS_DAY_CREATED", classDay, null, 201);
  }
}
