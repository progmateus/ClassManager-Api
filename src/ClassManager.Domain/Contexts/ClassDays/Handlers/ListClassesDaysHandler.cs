using AutoMapper;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Shared.Commands;
using ClassManager.Domain.Shared.Services.AccessControlService;
using ClassManager.Shared.Commands;

namespace ClassManager.Domain.Contexts.ClassDays.Handlers;

public class ListClassesDaysHandler
{
  private readonly IClassDayRepository _classDayRepository;
  private readonly IUsersRolesRepository _usersRolesRepository;
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly ISubscriptionRepository _subscriptionsRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  public ListClassesDaysHandler(
    IClassDayRepository classRepository,
    IUsersRolesRepository usersRolesRepository,
    IStudentsClassesRepository studentsClassesRepository,
    ISubscriptionRepository subscriptionsRepository,
    IMapper mapper,
    IAccessControlService accessControlService
    )
  {
    _classDayRepository = classRepository;
    _usersRolesRepository = usersRolesRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _subscriptionsRepository = subscriptionsRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid? tenantId, DateTime date)
  {

    if (tenantId.HasValue)
    {
      if (await _accessControlService.HasUserRoleAsync(loggedUserId, tenantId.Value, "admin"))
      {
        var classesDaysFound = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate([tenantId.Value], null, date));
        return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDaysFound, null, 200);
      }
      return new CommandResult(true, "CLASSES_DAYS_LISTED", Array.Empty<string>(), null, 200);
    }
    var usersubscriptions = await _subscriptionsRepository.ListSubscriptions([loggedUserId], []);

    var activesubscriptions = usersubscriptions.Where(x => x.Status == ESubscriptionStatus.ACTIVE).ToList();

    var tenantsIds = activesubscriptions.Select(s => s.TenantId).ToList();

    var userClasses = await _studentsClassesRepository.ListByUserOrClassOrTenantAsync([loggedUserId], tenantsIds, null);

    var classesIds = userClasses.Select(o => o.ClassId).ToList();

    var classesDays = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate(null, classesIds, date));

    return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDays, null, 200);
  }
}
