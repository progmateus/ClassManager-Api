using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Tenants.Repositories;

public class TenantRepository : Repository<Tenant>, ITenantRepository
{
  public TenantRepository(AppDbContext context) : base(context) { }

  public async Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Document.Number == document, cancellationToken);
  }

  public async Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Email.Address == email, cancellationToken);
  }

  public async Task<Tenant?> FindByStripeAccountId(string stripeAccountId, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.StripeAccountId == stripeAccountId, cancellationToken);
  }

  public async Task<Tenant?> FindByStripeSubscriptionId(string stripeSubscriptionId, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Subscriptions.Any(x => x.StripeSubscriptionId == stripeSubscriptionId), cancellationToken);
  }

  public async Task<List<Tenant>> GetActiveTenantsAsync()
  {
    return await DbSet
        .AsNoTracking()
        .Where(x => x.Status == ETenantStatus.ACTIVE)
        .ToListAsync();
  }

  public async Task<Tenant?> GetByEmailAsync(string email, CancellationToken cancellationToken)
  {
    return await DbSet
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Email.Address == email, cancellationToken);
  }

  public async Task<Tenant?> GetByIdAndIncludePlanAsync(Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet
    .Include(u => u.Plan)
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == tenantId, cancellationToken);
  }

  public async Task<List<Tenant>> SearchAsync(int skip, int limit, string search = "")
  {
    return await DbSet
    .AsNoTracking()
    .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search) || x.Username.Contains(search))
    .Where(x => x.Status != ETenantStatus.DELETED)
    .Skip(skip)
    .Take(limit)
    .ToListAsync();
  }

  public async Task<bool> UsernameAlreadyExistsAsync(string username, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Username == username, cancellationToken);
  }
}
