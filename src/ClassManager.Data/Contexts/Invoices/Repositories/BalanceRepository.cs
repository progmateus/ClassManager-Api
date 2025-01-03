using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class BalanceRepository : TRepository<Balance>, IBalanceRepository
{
  public BalanceRepository(AppDbContext context) : base(context) { }
}
