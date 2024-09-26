using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Data.Contexts.Plans.Repositories;

public class TimeTableRepository : TRepository<TimeTable>, ITimeTableRepository
{
  public TimeTableRepository(AppDbContext context) : base(context) { }

}
