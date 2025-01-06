using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;

public interface IInvoiceRepository : ITRepository<Invoice>
{
  Task<int> CountUserPendingInvoicesUntilDate(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken);
  Task<Invoice?> FindUserInvoiceById(Guid invoiceId, Guid tenantId, CancellationToken cancellationToken);
  Task<Invoice?> FindByStripeInvoiceId(string stripeInvoiceId);
  Task<List<Invoice>> ListByUserIdAndTenantId(Guid? tenantId, Guid? userId, Guid? subscriptionId, List<ETargetType>? targetTypes, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default);
}