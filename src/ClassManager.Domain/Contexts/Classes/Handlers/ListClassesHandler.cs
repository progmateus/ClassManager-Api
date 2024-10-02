using AutoMapper;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.Classes.Handlers;

public class ListClassesHandler
{
  private readonly IClassRepository _classRepository;
  private readonly IMapper _mapper;
  public ListClassesHandler(
    IClassRepository classRepository,
    IMapper mapper
    )
  {
    _classRepository = classRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(Guid tenantId)
  {
    var classes = _mapper.Map<List<ClassViewModel>>(await _classRepository.ListByTenantId(tenantId, new CancellationToken()));

    return new CommandResult(true, "CLASSES_LISTED", classes, null, 200);
  }
}
