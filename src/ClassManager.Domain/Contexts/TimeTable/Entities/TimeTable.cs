using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Shared.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Entities
{
  public class TimeTable : TenantEntity
  {
    public List<ScheduleDay>? SchedulesDays { get; set; } = [];
    public Tenant? Tenant { get; private set; }

    public TimeTable(Guid tenantId)
    {
      TenantId = tenantId;
    }

    protected TimeTable()
    {

    }
  }
}