using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class LinkRepository : TRepository<Link>, ILinkRepository
{
  public LinkRepository(AppDbContext context) : base(context) { }
}
