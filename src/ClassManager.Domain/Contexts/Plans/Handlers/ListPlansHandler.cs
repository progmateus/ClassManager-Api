using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class ListPlansHandler
{
  private readonly IPlanRepository _repository;

  public ListPlansHandler(
    IPlanRepository planRepository
    )
  {
    _repository = planRepository;
  }

  public async Task<ICommandResult> Handle()
  {

    var plans = await _repository.GetAllAsync(new CancellationToken());

    return new CommandResult(true, "Plans listed", plans, null, 204);
  }
}
