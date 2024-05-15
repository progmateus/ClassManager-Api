using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface ITenantListHandler
  {
    Task<ICommandResult> Handle(Guid tenantId);
  }
}
