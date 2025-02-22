using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.TimesTables.ViewModels
{
  public class TimeTableViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IList<ScheduleDayViewModel> SchedulesDays { get; set; } = [];
    public TenantViewModel? Tenant { get; private set; }

  }
}