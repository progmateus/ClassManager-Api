using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;

namespace ClassManager.Domain.Contexts.Plans.Handlers;

public class ListPlansHandler : IPaginationHandler<PaginationCommand>
{
  private readonly IPlanRepository _repository;

  public ListPlansHandler(
    IPlanRepository planRepository
    )
  {
    _repository = planRepository;
  }

  public async Task<ICommandResult> Handle(Guid loggedUserId, PaginationCommand command)
  {

    var plans = await _repository.List(new CancellationToken());

    return new CommandResult(true, "PLANS_LISTED", plans, null, 204);
  }
}
