using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;

namespace ClassManager.Domain.Contexts.Tenants.Repositories.Contracts
{
  public interface ITenantRepository : IRepository<Tenant>
  {
    Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken);
    Task<bool> UsernameAlreadyExistsAsync(string username, CancellationToken cancellationToken);
    Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken);
    Task<Tenant?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Tenant?> GetByIdAndIncludePlanAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<List<Tenant>> SearchAsync(string search);

  }
}
