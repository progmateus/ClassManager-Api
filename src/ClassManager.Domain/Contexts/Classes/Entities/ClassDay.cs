using System;
using ClassManager.Domain.Shared.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.Classes.Entities
{
  public class ClassDay : Entity
  {
    public ClassDay(DateTime date, string start, string end, string? observation)
    {
      Date = date;
      HourStart = start;
      HourEnd = end;
      Observation = observation;
    }

    protected ClassDay()
    {

    }

    public DateTime Date { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public EClassDayStatus Status { get; private set; } = EClassDayStatus.PENDING;
    public string? Observation { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    public void ChangeStatus(EClassDayStatus status)
    {
      Status = status;
    }
  }
}