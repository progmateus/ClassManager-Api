

using ClassManager.Domain.Contexts.Addresses.Entites;

namespace ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
public interface IAddressRepository : IRepository<Address>
{
  Task<List<Address>> ListByTenantIdAsync(Guid tenantId);
}