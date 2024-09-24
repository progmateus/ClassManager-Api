using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Shared.Handlers
{
  public interface ITenantDeleteActionHandler
  {
    Task<ICommandResult> Handle(Guid loggedUserId, Guid tenantId, Guid entityId);
  }
}
