using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.TimesTables.Entities;

public class TimeTable : TenantEntity
{
  public string Name { get; set; } = string.Empty!;
  public List<ScheduleDay>? SchedulesDays { get; set; } = [];
  public Tenant? Tenant { get; private set; }

  public TimeTable(Guid tenantId, string name)
  {
    TenantId = tenantId;
    Name = name;
  }

  protected TimeTable()
  {

  }
}
