using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ClassManager.Domain.Libs.MassTransit.Events;

public sealed class RefreshClassClassesDaysEventConsumer : IConsumer<RefreshClassClassesDaysEvent>
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IClassRepository _classRepository;
  private readonly ILogger<RefreshClassClassesDaysEventConsumer> _logger;


  public RefreshClassClassesDaysEventConsumer(
    IClassDayRepository classDayRepository,
    ILogger<RefreshClassClassesDaysEventConsumer> logger,
    IClassRepository classRepository
  )
  {
    _classDayRepository = classDayRepository;
    _logger = logger;
    _classRepository = classRepository;
  }
  public async Task Consume(ConsumeContext<RefreshClassClassesDaysEvent> context)
  {
    try
    {
      _logger.LogInformation("Job RefreshClassClassesDaysEventConsumer initialized");

      var dates = new List<DateTime>();
      var classesDays = new List<ClassDay>();

      var dateNow = DateTime.Now;

      var classEntity = await _classRepository.FindByIdWithTimeTable(context.Message.classId, new CancellationToken());

      if (classEntity is null)
      {
        throw new Exception("ERR_CLASS_NOT_FOUND");
      }

      // gera um array com todos os dias do mes
      for (var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day); date.Month == dateNow.Month; date = date.AddDays(1))
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

      foreach (var datesGroupedByWeekDay in groupedDaysByWeekDayList)
      {
        var schedulesInCurrentWeekDay = classEntity.TimeTable.SchedulesDays.Select(x => x).Where(x => (int)x.WeekDay == (int)datesGroupedByWeekDay.DayOfWeek).ToList();
        foreach (var schedule in schedulesInCurrentWeekDay)
        {
          var hourStart = schedule.HourStart.Split(":")[0];
          var minStart = schedule.HourStart.Split(":")[1];

          foreach (var date in datesGroupedByWeekDay.Dates)
          {
            var dateGenerated = date.AddHours(int.Parse(hourStart)).AddMinutes(int.Parse(minStart));

            var classDay = new ClassDay(dateGenerated, schedule.HourStart, schedule.HourEnd, classEntity.Id);
            classesDays.Add(classDay);
          }
        }
      }
      await _classDayRepository.CreateRangeAsync(classesDays, new CancellationToken());
      _logger.LogInformation("Job RefreshClassClassesDaysEventConsumer finished");
    }
    catch (Exception err)
    {
      _logger.LogInformation($"Job RefreshClassClassesDaysEventConsumer error: {err.Message}");
      throw new Exception(err.Message);
    }
  }
}