using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;


public interface IInvoiceRepository : ITRepository<Invoice>
{
  Task<Subscription> CountUserPendingInvoices(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}