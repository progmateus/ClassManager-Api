using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.TimesTables.Entities;

public class TimeTable : TenantEntity
{

  public TimeTable(Guid tenantId, string name)
  {
    TenantId = tenantId;
    Name = name;
  }

  protected TimeTable()
  {

  }
  public string Name { get; set; } = string.Empty!;
  public List<ScheduleDay> SchedulesDays { get; set; } = [];
  public Tenant? Tenant { get; private set; }
  public IList<Class> Classes { get; } = [];

  public void SetName(string name)
  {
    Name = name;
  }
}
