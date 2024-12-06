using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Addresses.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
  public AddressRepository(AppDbContext context) : base(context) { }

  public async Task<List<Address>> ListByTenantIdAsync(Guid tenantId)
  {
    return await DbSet.Where(x => x.TenantId == tenantId).ToListAsync();
  }
}
