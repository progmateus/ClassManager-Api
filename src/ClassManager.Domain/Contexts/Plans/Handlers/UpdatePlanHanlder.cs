using ClassManager.Domain.Contexts.Plans.Commands;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class UpdatePlandHandler : Notifiable, IActionHandler<PlanCommand>
{
  private readonly IPlanRepository _repository;
  public UpdatePlandHandler(
    IPlanRepository planRepository
    )
  {
    _repository = planRepository;
  }
  public async Task<ICommandResult> Handle(Guid id, PlanCommand command)
  {

    command.Validate();
    if (command.Invalid)
    {
      AddNotifications(command);
      return new CommandResult(false, "ERR_VALIDATION", null, command.Notifications);
    }

    var plan = await _repository.GetByIdAsync(id, new CancellationToken());

    if (plan is null)
    {
      return new CommandResult(false, "ERR_PLAN_NOT_FOUND", null, null, 404);
    }

    plan.ChangePlan(command.Name, command.Description, command.StudentsLimit, command.ClassesLimit, command.Price);

    await _repository.UpdateAsync(plan, new CancellationToken());

    return new CommandResult(true, "PLAN_UPDATED", plan, null, 200);
  }
}
