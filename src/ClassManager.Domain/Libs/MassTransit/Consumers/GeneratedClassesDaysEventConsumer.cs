using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using MassTransit;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed class GeneratedClassesDaysEventConsumer : IConsumer<GeneratedClassesDaysEvent>
{
  private readonly IClassDayRepository _classDayRepository;

  public GeneratedClassesDaysEventConsumer(IClassDayRepository classDayRepository)
  {
    _classDayRepository = classDayRepository;
  }
  public async Task Consume(ConsumeContext<GeneratedClassesDaysEvent> context)
  {
    try
    {
      var dates = new List<DateTime>();
      var classesDays = new List<ClassDay>();

      var year = context.Message.year;
      var month = context.Message.month;
      var day = context.Message.day;
      var timesTables = context.Message.timesTables;

      // gera um array com todos os dias do mes
      for (var date = new DateTime(year, month, day); date.Month == month; date = date.AddDays(1))
      {
        dates.Add(date);
      }

      // agrupa o array pelo dia da semana
      var groupedDaysByWeekDayList = dates
      .GroupBy(d => d.DayOfWeek)
      .Select(grp => new
      {
        DayOfWeek = grp.Key,
        Dates = grp.ToList()
      })
      .ToList();

      foreach (var timeTable in timesTables)
      {
        foreach (var timeTableClass in timeTable.Classes)
        {
          foreach (var datesGroupedByWeekDay in groupedDaysByWeekDayList)
          {
            var schedulesInCurrentWeekDay = timeTable.SchedulesDays.Select(x => x).Where(x => (int)x.WeekDay == (int)datesGroupedByWeekDay.DayOfWeek).ToList();
            foreach (var schedule in schedulesInCurrentWeekDay)
            {
              var hourStart = schedule.HourStart.Split(":")[0];
              var minStart = schedule.HourStart.Split(":")[1];

              foreach (var date in datesGroupedByWeekDay.Dates)
              {
                var dateGenerated = date.AddHours(int.Parse(hourStart)).AddMinutes(int.Parse(minStart));

                var classDay = new ClassDay(dateGenerated, schedule.HourStart, schedule.HourEnd, timeTableClass.Id);
                classesDays.Add(classDay);
              }
            }
          }
        }
      }
      await _classDayRepository.CreateRangeAsync(classesDays, new CancellationToken());
    }
    catch (Exception err)
    {
      throw new Exception(err.Message);
    }
  }
}