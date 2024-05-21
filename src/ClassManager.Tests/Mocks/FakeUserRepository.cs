using System.Linq.Expressions;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;

namespace ClassManager.Tests.Mocks
{
  public class FakeUserRepository : IUserRepository
  {
    private readonly List<User> _users;
    public FakeUserRepository()
    {
      _users = Users;
    }

    public List<User> Users { get; private set; } = new();
    public async Task CreateAsync(User entity, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      _users.Add(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var user = await GetByIdAsync(id, cancellationToken);
      if (user is not null)
      {
        _users.Remove(user);
      }
    }

    public async Task<bool> DocumentAlreadyExistsAsync(string document, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      if (document == "999999999")
        return true;

      return false;
    }

    public async Task<bool> EmailAlreadyExtstsAsync(string email, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      if (email == "user@email.com")
        return true;

      return false;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      return _users;
    }

    public Task<IEnumerable<User>> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var user = _users.Find((u) => u.Id == id);
      return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var user = _users.Find((u) => u.Email == email);
      return user;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken)
    {
      await Task.Delay(100);
      var index = _users.FindIndex((u) => u.Id == entity.Id);
      _users[index] = entity;
    }

    public Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public List<User> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task CreateRangeAsync(List<User> entities, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(List<User> entities, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<User?> GetRolesByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    Task<List<User>> IRepository<User>.GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}