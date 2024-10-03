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
  private readonly IStudentsClassesRepository _studentsClassesRepository;
  private readonly ISubscriptionRepository _subscriptionsRepository;
  private readonly IMapper _mapper;
  private readonly IAccessControlService _accessControlService;
  private readonly IUsersRolesRepository _usersRolesRepository;
  public ListClassesDaysHandler(
    IClassDayRepository classRepository,
    IStudentsClassesRepository studentsClassesRepository,
    ISubscriptionRepository subscriptionsRepository,
    IMapper mapper,
    IAccessControlService accessControlService,
    IUsersRolesRepository usersRolesRepository
    )
  {
    _classDayRepository = classRepository;
    _studentsClassesRepository = studentsClassesRepository;
    _subscriptionsRepository = subscriptionsRepository;
    _mapper = mapper;
    _accessControlService = accessControlService;
    _usersRolesRepository = usersRolesRepository;
  }
  public async Task<ICommandResult> Handle(Guid loggedUserId, Guid? tenantId, DateTime date)
  {

    // se vier tenantId (seção da empresa)
    // busca apenas as aulas daquela empresa

    if (tenantId.HasValue)
    {
      if (await _accessControlService.HasUserAnyRoleAsync(loggedUserId, tenantId.Value, ["admin"]))
      {
        var classesDaysFound = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate([tenantId.Value], [], date));
        return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDaysFound, null, 200);
      }
      return new CommandResult(true, "CLASSES_DAYS_LISTED", Array.Empty<string>(), null, 200);
    }

    // se vier não vier tenantId (seção do usuário) verifica se o usuario é adm de alguma empresa
    // lista as aulas de todas as empresas dele

    var userAdminRoles = await _usersRolesRepository.GetByUserIdAndRoleName(loggedUserId, ["admin"]);

    if (userAdminRoles.Count > 0)
    {
      var adminTenantsIds = userAdminRoles.Select(s => s.TenantId).ToList();

      var classesDaysFound = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate(adminTenantsIds, [], date));
      return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDaysFound, null, 200);
    }

    // se o usuario não for adm de nenhuma empresa
    // lista de acordo com as inscrições dele

    var usersubscriptions = await _subscriptionsRepository.ListSubscriptions([loggedUserId], []);

    var activesubscriptions = usersubscriptions.Where(x => x.Status == ESubscriptionStatus.ACTIVE).ToList();

    var tenantsIds = activesubscriptions.Select(s => s.TenantId).ToList();

    var userClasses = await _studentsClassesRepository.ListByUserOrClassOrTenantAsync([loggedUserId], tenantsIds, []);

    var classesIds = userClasses.Select(o => o.ClassId).ToList();

    var classesDays = _mapper.Map<List<ClassDayViewModel>>(await _classDayRepository.ListByTenantOrClassAndDate([], classesIds, date));

    return new CommandResult(true, "CLASSES_DAYS_LISTED", classesDays, null, 200);
  }
}
