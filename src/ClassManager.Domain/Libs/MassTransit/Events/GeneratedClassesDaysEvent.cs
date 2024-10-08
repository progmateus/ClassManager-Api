using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed record GeneratedClassesDaysEvent(List<TimeTable> timesTables, int year, int month, int day)
{

}