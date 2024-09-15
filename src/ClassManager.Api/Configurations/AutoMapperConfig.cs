using AutoMapper;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Api.Configurations;
public class AutoMapperConfig : Profile
{
  public AutoMapperConfig()
  {
    CreateMap<User, UserViewModel>()
    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));

    CreateMap<Booking, BookingViewModel>();

    CreateMap<Class, ClassViewModel>();
    CreateMap<ClassDay, ClassDayViewModel>();
    CreateMap<StudentsClasses, StudentsClassesViewModel>();
    CreateMap<TeachersClasses, TeachersClassesViewModel>();

    CreateMap<Plan, PlanViewModel>();

    CreateMap<Role, RoleViewModel>();
    CreateMap<UsersRoles, UsersRolesViewModel>();

    CreateMap<Subscription, SubscriptionViewModel>();

    CreateMap<Tenant, TenantViewModel>()
  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
    CreateMap<TenantPlan, TenantPlanViewModel>();
  }
}