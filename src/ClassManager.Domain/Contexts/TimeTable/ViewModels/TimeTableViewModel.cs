using ClassManager.Domain.Contexts.tenants.ViewModels;

namespace ClassManager.Domain.Contexts.TimesTables.ViewModels
{
  public class TimeTableViewModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<ScheduleDayViewModel> SchedulesDays { get; set; } = [];
    public TenantPreviewViewModel? Tenant { get; private set; }

  }
}