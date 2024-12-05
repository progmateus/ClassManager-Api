using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Addresses.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
  public AddressRepository(AppDbContext context) : base(context) { }
}
