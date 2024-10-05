using ClassManager.Domain.Contexts.ClassDays.Commands;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;
using ClassManager.Shared.Handlers;
using Flunt.Notifications;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class GenerateClassesDaysHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IClassRepository _classRepository;
  private readonly ITenantRepository _tenantsRepository;
  private readonly IAccessControlService _accessControlService;
  private readonly ITimeTableRepository _timeTableRepository;

  public GenerateClassesDaysHandler(
    IClassDayRepository classDayRepository,
    IClassRepository classRepository,
    IAccessControlService accessControlService,
    ITenantRepository tenantsRepository,
    ITimeTableRepository timeTableRepository
    )
  {
    _classDayRepository = classDayRepository;
    _accessControlService = accessControlService;
    _classRepository = classRepository;
    _tenantsRepository = tenantsRepository;
    _timeTableRepository = timeTableRepository;
  }
  public async Task<ICommandResult> Handle()
  {
    var timesTables = await _timeTableRepository.GetByActiveTenants();

    int year = 2024;
    int month = 10;

    var dates = new List<DateTime>();

    for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
    {
      dates.Add(date);
    }

    var groupedCustomerList = dates
    .GroupBy(d => d.DayOfWeek)
    .Select(grp => new
    {
      DayOfWeek = grp.Key,
      Dates = grp.ToList()
    })
    .ToList();

    foreach (var timeTable in timesTables)
    {
      Console.WriteLine(timeTable);
    }


    return new CommandResult(true, "GENERATE", groupedCustomerList, null, 200);

  }
}

class DateObject
{
  public DateTime Date { get; set; }
}
