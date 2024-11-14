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

    if (command.Page < 1) command.Page = 1;

    var skip = (command.Page - 1) * command.Limit;

    var plans = await _repository.ListWithPaginationAsync(command.Search, skip, command.Limit, new CancellationToken());

    return new CommandResult(true, "PLANS_LISTED", plans, null, 204);
  }
}
