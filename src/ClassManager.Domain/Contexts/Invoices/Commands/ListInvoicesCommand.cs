using ClassManager.Domain.Shared.Commands;

namespace ClasManager.Domain.Contexts.Invoices.Commands;

public class ListInvoicesCommand : PaginationCommand
{
  public Guid? UserId { get; set; }
  public Guid? TenantId { get; set; }
}