using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class GenerateClassesDaysHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly ITimeTableRepository _timeTableRepository;

  public GenerateClassesDaysHandler(
    ITimeTableRepository timeTableRepository,
    IClassDayRepository classDayRepository
    )
  {
    _timeTableRepository = timeTableRepository;
    _classDayRepository = classDayRepository;
  }
  public async Task<ICommandResult> Handle()
  {

    var timesTables = await _timeTableRepository.GetByActiveTenants();

    int year = DateTime.Now.Year;
    int month = DateTime.Now.Month;

    var dates = new List<DateTime>();
    var classesDays = new List<ClassDay>();

    // gera um array com todos os dias do mes
    for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
    {
      if (date > DateTime.Now)
      {
        dates.Add(date);

      }
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
        foreach (var grouped in groupedDaysByWeekDayList)
        {
          var schedulesInCurrentWeekDay = timeTable.SchedulesDays.Select(x => x).Where(x => (int)x.WeekDay == (int)grouped.DayOfWeek).ToList();
          foreach (var schedule in schedulesInCurrentWeekDay)
          {
            var hourStart = schedule.HourStart.Split(":")[0];
            var minStart = schedule.HourStart.Split(":")[1];

            foreach (var date in grouped.Dates)
            {
              var dateGenerated = date.AddHours(int.Parse(hourStart)).AddMinutes(int.Parse(minStart)).ToUniversalTime();

              var classDay = new ClassDay(schedule.Name, dateGenerated, schedule.HourStart, schedule.HourEnd, timeTableClass.Id);
              classesDays.Add(classDay);
            }
          }
        }
      }
    }

    await _classDayRepository.CreateRangeAsync(classesDays, new CancellationToken());
    return new CommandResult(true, "GENERATED", classesDays, null, 200);
  }

  class DateObject
  {
    public DayOfWeek DayOfWeek { get; set; }
    public List<DateTime> Dates { get; set; } = [];
  }
}
