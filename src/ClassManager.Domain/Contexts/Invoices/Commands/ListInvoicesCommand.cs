using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Shared.Commands;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class ListInvoicesCommand : PaginationCommand
{
  public Guid? UserId { get; set; }
  public Guid? TenantId { get; set; }
  public Guid? SubscriptionId { get; set; }
  public List<ETargetType>? TargetTypes { get; set; } = [];
}