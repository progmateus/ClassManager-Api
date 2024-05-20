using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class ClassDay : Entity
  {
    public ClassDay(DateTime date, string hourStart, string hourEnd, Guid classId)
    {
      Date = date;
      HourStart = hourStart;
      HourEnd = hourEnd;
      ClassId = classId;
    }

    protected ClassDay()
    {

    }

    public DateTime Date { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public EClassDayStatus Status { get; private set; } = EClassDayStatus.PENDING;
    public string? Observation { get; private set; }
    public Guid ClassId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Class Class { get; private set; }


    public void ChangeStatus(EClassDayStatus status, string? observation)
    {
      Status = status;
      Observation = observation;
    }
  }
}