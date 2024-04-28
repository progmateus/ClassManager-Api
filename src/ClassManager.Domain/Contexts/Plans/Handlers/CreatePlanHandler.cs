using ClassManager.Domain.Contexts.Plans.Commands;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class CreatePlandHandler : Notifiable, IHandler<PlanCommand>
{
  private readonly IPlanRepository _repository;
  public CreatePlandHandler(
    IPlanRepository planRepository
    )
  {
    _repository = planRepository;
  }
  public async Task<ICommandResult> Handle(PlanCommand command)
  {
    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "Plan not Created", null, command.Notifications);
    }

    var plan = new Plan(command.Name, command.Description, command.StudentsLimit, command.ClassesLimit, command.Price);

    await _repository.CreateAsync(plan, new CancellationToken());

    return new CommandResult(true, "Plan created", plan, null, 201);
  }
}
