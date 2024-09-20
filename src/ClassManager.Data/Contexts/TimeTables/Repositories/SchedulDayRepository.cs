using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class ScheduleDayRepository : TRepository<ScheduleDay>, IScheduleDayRepository
{
  public ScheduleDayRepository(AppDbContext context) : base(context) { }
}
