using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class GetClassByIdHandler
{
  private readonly IClassRepository _classRepository;
  public GetClassByIdHandler(
    IClassRepository classRepository
    )
  {
    _classRepository = classRepository;
  }
  public async Task<ICommandResult> Handle(Guid tenantId, Guid classId)
  {
    var classFound = await _classRepository.GetByIdAndTenantId(tenantId, classId, new CancellationToken());

    return new CommandResult(true, "CLASS_GOTTEN", classFound, null, 200);
  }
}
