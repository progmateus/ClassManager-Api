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

  public async Task<StripeCustomer?> FindByUserIdAndTenantId(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await DbSet.FirstOrDefaultAsync(x => x.UserId == userId && x.TenantId == tenantId);
  }
}