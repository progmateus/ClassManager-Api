using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class DeletePlanHandler
{
  private readonly IPlanRepository _repository;

  public DeletePlanHandler(
    IPlanRepository planRepository
    )
  {
    _repository = planRepository;
  }

  public async Task<ICommandResult> Handle(Guid id)
  {

    if (await _repository.GetByIdAsync(id, default) == null)
    {
      return new CommandResult(false, "Plan not found", null, null, 404);
    }


    await _repository.DeleteAsync(id, default);

    return new CommandResult(true, "Plan deleted", null, null, 204);
  }
}
