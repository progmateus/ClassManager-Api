using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;


public interface ITenantInvoiceRepository : ITRepository<TenantInvoice>
{
  Task<TenantInvoice> CountUserPendingInvoices(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}