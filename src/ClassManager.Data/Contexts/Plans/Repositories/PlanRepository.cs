using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.Repositories;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class PlanRepository : Repository<Plan>, IPlanRepository
{
  public PlanRepository(AppDbContext context) : base(context) { }
}
