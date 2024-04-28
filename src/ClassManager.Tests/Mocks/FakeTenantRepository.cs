using System.Linq.Expressions;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;

namespace ClassManager.Tests.Mocks
{
  public class FakeTenantRepository : ITenantRepository
  {
    private readonly List<Tenant> _tenants;
    public FakeTenantRepository()
    {
      _tenants = Tenants;
    }

    public List<Tenant> Tenants { get; private set; } = new();

    public async Task CreateAsync(Tenant entity, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      _tenants.Add(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var tenant = await GetByIdAsync(id, cancellationToken);
      if (tenant is not null)
      {
        _tenants.Remove(tenant);
      }
    }

    public async Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      if (document == "12312312312312")
        return true;

      return false;
    }

    public async Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      if (email == "tenant@email.com")
        return true;

      return false;
    }

    public async Task<List<Tenant>> GetAllAsync(CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      return _tenants;
    }

    public Task<IEnumerable<Tenant>> GetAsync(Expression<Func<Tenant, bool>> predicate, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var tenant = _tenants.Find((u) => u.Id == id);
      return tenant;
    }

    public async Task<Tenant?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var tenant = _tenants.Find((u) => u.Email == email);
      return tenant;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task UpdateAsync(Tenant entity, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var index = _tenants.FindIndex((u) => u.Id == entity.Id);
      _tenants[index] = entity;
    }
  }
}