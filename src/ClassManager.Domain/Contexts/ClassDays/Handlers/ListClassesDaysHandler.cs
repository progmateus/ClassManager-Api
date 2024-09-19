using AutoMapper;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class ListClassesDaysHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly ISubscriptionRepository _subscriptionsRepository;
  private readonly IMapper _mapper;
  public ListClassesDaysHandler(
    IClassDayRepository classRepository,
    IUsersRolesRepository usersRolesRepository,
    IStudentsClassesRepository studentsClassesRepository,
    ISubscriptionRepository subscriptionsRepository,
    IMapper mapper
    )
  {
    _classDayRepository = classRepository;
    _usersRolesRepository = usersRolesRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _subscriptionsRepository = subscriptionsRepository;
    _mapper = mapper;
  }
  public async Task<ICommandResult> Handle(List<Guid>? usersIds, Guid? tenantId, DateTime date)
  {

    if (tenantId.HasValue)
    {
      var classesDaysFound = _classDayRepository.ListByTenantId(tenantId.Value);
      return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDaysFound, null, 200);
    }
    var usersubscriptions = await _subscriptionsRepository.ListSubscriptions(usersIds, []);

    var activesubscriptions = usersubscriptions.Where(x => x.Status == ESubscriptionStatus.ACTIVE).ToList();

    var tenantsIds = activesubscriptions.Select(s => s.TenantId).ToList();

    var userClasses = await _studentsClassesRepository.ListByUserOrClassOrTenantAsync(usersIds, tenantsIds, null);

    var classesIds = userClasses.Select(o => o.ClassId).ToList();

    var classesDays = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate(null, classesIds, date));

    return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDays, null, 200);
  }
}
