using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListClassesHandler
{
  private readonly IClassRepository _classRepository;
  public ListClassesHandler(
    IClassRepository classRepository
    )
  {
    _classRepository = classRepository;
  }
  public ICommandResult Handle(Guid tenantId)
  {
    var tenantPlans = _classRepository.ListByTenantId(tenantId, new CancellationToken());

    return new CommandResult(true, "CLASSES_LISTED", tenantPlans, null, 200);
  }
}
