using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Entities;

namespace ClassManager.Domain.Contexts.ClassDays.Helpers;

public class GenerateClassesDaysHelper
{
  private readonly IClassDayRepository _classDayRepository;

  public GenerateClassesDaysHelper(
    IClassDayRepository classDayRepository
    )
  {
    _classDayRepository = classDayRepository;
  }
  public async Task Handle(List<TimeTable> timesTables, int year, int month, int day)
  {
    var dates = new List<DateTime>();
    var classesDays = new List<ClassDay>();

    // gera um array com todos os dias do mes
    for (var date = new DateTime(year, month, day); date.Month == month; date = date.AddDays(1))
    {
      dates.Add(date);
    }

    // agrupa o array pelo dia da semana
    var groupedDaysByWeekDayList = dates
    .GroupBy(d => d.DayOfWeek)
    .Select(grp => new DateObject
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

              var classDay = new ClassDay(schedule.Name, dateGenerated, schedule.HourStart, schedule.HourEnd, timeTableClass.Id);
              classesDays.Add(classDay);
            }
          }
        }
      }
    }
    await _classDayRepository.CreateRangeAsync(classesDays, new CancellationToken());
  }
}

class DateObject
{
  public DayOfWeek DayOfWeek { get; set; }
  public List<DateTime> Dates { get; set; } = [];
}
