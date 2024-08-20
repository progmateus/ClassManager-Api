using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class GetClassDayByIdHandler
{
  private readonly IClassDayRepository _classDayRepository;
  public GetClassDayByIdHandler(
    IClassDayRepository classRepository
    )
  {
    _classDayRepository = classRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classDayId)
  {
    var classFound = await _classDayRepository.GetByIdAndTenantIdAsync(tenantId, classDayId);

    return new CommandResult(true, "CLASS_DAY_GOTTEN", classFound, null, 200);
  }
}
