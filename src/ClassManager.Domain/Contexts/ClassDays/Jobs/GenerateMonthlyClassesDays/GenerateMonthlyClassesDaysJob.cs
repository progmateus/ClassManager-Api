using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Libs.MassTransit.Events;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ClasManager.Domain.Contexts.ClassDays.Jobs.GenerateMonthlyClassesDays;

[DisallowConcurrentExecution]
public class GenerateMonthlyClassesDaysJob : IJob
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly ITimeTableRepository _timeTableRepository;
  private readonly ILogger<GenerateMonthlyClassesDaysJob> _logger;
  public GenerateMonthlyClassesDaysJob(
    IClassDayRepository classDayRepository,
    ILogger<GenerateMonthlyClassesDaysJob> logger,
    ITimeTableRepository timeTableRepository
  )
  {
    _classDayRepository = classDayRepository;
    _logger = logger;
    _timeTableRepository = timeTableRepository;
  }
  public async Task Execute(IJobExecutionContext context)
  {
    try
    {
      _logger.LogInformation("Job initialized");
      var dates = new List<DateTime>();
      var classesDays = new List<ClassDay>();

      var dateNow = DateTime.Now;

      var year = dateNow.Year;
      var nextMonth = dateNow.AddMonths(1).Month;
      var initialDay = 1;

      var timesTables = await _timeTableRepository.GetByActiveTenants();

      // gera um array com todos os dias do mes
      for (var date = new DateTime(year, nextMonth, initialDay); date.Month == nextMonth; date = date.AddDays(1))
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

                var classDay = new ClassDay(schedule.Name, dateGenerated, schedule.HourStart, schedule.HourEnd, timeTableClass.Id);
                classesDays.Add(classDay);
              }
            }
          }
        }
      }
      await _classDayRepository.CreateRangeAsync(classesDays, new CancellationToken());
      _logger.LogInformation("Job GenerateMonthlyClassesDaysJob finished");
    }
    catch (Exception err)
    {
      _logger.LogInformation($"Job GenerateMonthlyClassesDaysJob error: {err.Message}");
      throw new Exception(err.Message);
    }
  }
}