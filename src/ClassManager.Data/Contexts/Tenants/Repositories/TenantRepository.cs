using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
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
}
