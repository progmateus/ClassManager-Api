using System.Linq.Expressions;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;

namespace ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;


public interface IInvoiceRepository : IRepository<Invoice>
{
  Task<Subscription> CountUserPendingInvoices(Guid userId, Guid tenantId, CancellationToken cancellationToken);
}