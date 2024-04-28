using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace classManager.Data.Contexts.Roles.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
  public RoleRepository(AppDbContext context) : base(context) { }
  public async Task<bool> NameAlreadyExists(string name, CancellationToken cancellationToken)
  {
    return await DbSet.AsNoTracking().AnyAsync(x => x.Name == name, cancellationToken);
  }
}