using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using Microsoft.EntityFrameworkCore;

public class InvoiceRepository : TRepository<Invoice>, IInvoiceRepository
{
  public InvoiceRepository(DbContext dbContext) : base(dbContext)
  {
  }

  public async Task<int> CountUserPendingInvoicesUntilDate(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .CountAsync((invoice) => invoice.UserId == userId && invoice.TargetType == EInvoiceTarget.USER && invoice.Type == EInvoiceType.USER_SUBSCRIPTION && invoice.ExpiresAt > initialDate && invoice.ExpiresAt < finalDate);
  }
}