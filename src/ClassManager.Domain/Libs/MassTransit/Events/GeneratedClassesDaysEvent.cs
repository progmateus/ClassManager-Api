using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed record GeneratedClassesDaysEvent(List<Guid> timesTablesIds, int year, int month, int day)
{

}