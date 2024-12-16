using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class InvoiceRepository : TRepository<Invoice>, IInvoiceRepository
{
  public InvoiceRepository(AppDbContext context) : base(context) { }

  public async Task<int> CountUserPendingInvoicesUntilDate(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken)
  {
    return await DbSet
      .CountAsync((invoice) => invoice.UserId == userId && invoice.TargetType == EInvoiceTargetType.USER && invoice.Type == EInvoiceType.USER_SUBSCRIPTION && invoice.ExpiresAt > initialDate && invoice.ExpiresAt < finalDate);
  }

  public async Task<Invoice?> FindByStripeInvoiceId(string stripeInvoiceId)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.StripeInvoiceId == stripeInvoiceId);
  }

  public async Task<Invoice?> FindUserInvoiceById(Guid invoiceId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.Id == invoiceId && x.TenantId == tenantId && x.TargetType == EInvoiceTargetType.USER);
  }

  public async Task<List<Invoice>> ListByUserIdAndTenantId(Guid? tenantId, Guid? userId, Guid? subscriptionId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    Console.WriteLine("========================");
    Console.WriteLine("========================");
    Console.WriteLine("========================");
    Console.WriteLine("========================");
    Console.WriteLine(tenantId);
    Console.WriteLine(userId);
    Console.WriteLine(subscriptionId);
    return await DbSet
    .Include(x => x.Tenant)
    .Where(x => !tenantId.HasValue || x.TenantId == tenantId.Value)
    .Where(x => !userId.HasValue || x.UserId == userId.Value)
    .Where(x => !subscriptionId.HasValue || x.SubscriptionId == subscriptionId.Value)
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }
}
