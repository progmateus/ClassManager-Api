using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class InvoiceRepository : TRepository<Invoice>, IInvoiceRepository
{
  public InvoiceRepository(AppDbContext context) : base(context) { }

  public async Task<int> CountUserPendingInvoicesUntilDate(Guid userId, Guid tenantId, DateTime initialDate, DateTime finalDate, CancellationToken cancellationToken)
  {
    return await DbSet
      .CountAsync((invoice) => invoice.UserId == userId && invoice.TargetType == ETargetType.USER && invoice.Type == EInvoiceType.SUBSCRIPTION && invoice.ExpiresAt > initialDate && invoice.ExpiresAt < finalDate);
  }

  public async Task<Invoice?> FindByStripeInvoiceId(string stripeInvoiceId)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.StripeInvoiceId == stripeInvoiceId);
  }

  public async Task<Invoice?> FindUserInvoiceById(Guid invoiceId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.Id == invoiceId && x.TenantId == tenantId && x.TargetType == ETargetType.USER);
  }

  public async Task<bool> HasSubscriptionUnpaidInvoice(Guid tenantId, Guid userId, ETargetType targetType, CancellationToken cancellationToken)
  {

    var status = new List<EInvoiceStatus>([EInvoiceStatus.OPEN, EInvoiceStatus.VOID, EInvoiceStatus.UNPAID, EInvoiceStatus.UNCOLLECTIBLE]);

    return await
      DbSet.AnyAsync(
      x => x.TenantId == tenantId
        && x.UserId == userId && x.TargetType == targetType
          && x.Type == EInvoiceType.SUBSCRIPTION
            && status.Contains(x.Status),
      cancellationToken
    );
  }

  public async Task<List<Invoice>> ListByUserIdAndTenantId(Guid? tenantId, Guid? userId, Guid? subscriptionId, List<ETargetType>? targetTypes, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
    .Include(x => x.Tenant)
    .Include(x => x.Plan)
    .Include(x => x.TenantPlan)
    .Where(x => !tenantId.HasValue || x.TenantId == tenantId.Value)
    .Where(x => !userId.HasValue || x.UserId == userId.Value)
    .Where(x => !subscriptionId.HasValue || x.SubscriptionId == subscriptionId.Value)
    .Where(x => targetTypes.IsNullOrEmpty() || targetTypes.Contains(x.TargetType))
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }
}
