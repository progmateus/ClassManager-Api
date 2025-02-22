using ClassManager.Domain.Contexts.Plans.Entities;

namespace ClassManager.Domain.Contexts.Plans.Repositories;

public interface IPlanRepository : IRepository<Plan>
{
  Task<Plan?> FindByStripePriceId(string stripePriceId, CancellationToken cancellationToken = default);
}