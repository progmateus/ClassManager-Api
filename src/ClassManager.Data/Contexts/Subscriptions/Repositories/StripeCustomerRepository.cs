using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace classManager.Data.Contexts.Subscriptions.Repositories;

public class StripeCustomerRepository : TRepository<StripeCustomer>, IStripeCustomerRepository
{
  public StripeCustomerRepository(AppDbContext context) : base(context) { }

  public async Task<StripeCustomer?> FindByCustomerId(string customerId, CancellationToken cancellationToken = default)
  {
    return await DbSet.Include(x => x.Tenant).ThenInclude(t => t.Plan).FirstOrDefaultAsync(x => x.StripeCustomerId == customerId, cancellationToken);
  }

  public async Task<StripeCustomer?> FindByUserIdAndTenantIdAndType(Guid? userId, Guid tenantId, ETargetType type, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => (!userId.HasValue || x.UserId == userId) && x.TenantId == tenantId && x.TargetType == type, cancellationToken);
  }
}