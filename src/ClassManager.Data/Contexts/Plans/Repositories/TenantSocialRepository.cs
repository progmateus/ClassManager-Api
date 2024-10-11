using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TenantSocialRepository : TRepository<TenantSocial>, ITenantSocialRepository
{
  public TenantSocialRepository(AppDbContext context) : base(context) { }
}
