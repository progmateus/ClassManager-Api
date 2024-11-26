using AutoMapper;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.ClassDays.ViewModels;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Invoices.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Plans.ViewModels;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Roles.ViewModels;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Subscriptions.ViewModels;
using ClassManager.Domain.Contexts.tenants.ViewModels;
using ClassManager.Domain.Contexts.Tenants.Entities;
using ClassManager.Domain.Contexts.Tenants.ViewModels;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using ClassManager.Domain.Contexts.TimesTables.ViewModels;
using ClassManager.Domain.Contexts.Users.ViewModels;

namespace ClassManager.Api.Configurations;
public class AutoMapperConfig : Profile
{
  public AutoMapperConfig()
  {
    CreateMap<User, UserPreviewViewModel>()
    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name.FirstName} {src.Name.LastName}"))
    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address)).PreserveReferences();

    CreateMap<User, UserProfileViewModel>()
    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name.FirstName} {src.Name.LastName}"))
    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document.Number)).PreserveReferences();

    CreateMap<Booking, BookingViewModel>().PreserveReferences();


    CreateMap<Class, ClassViewModel>().PreserveReferences();
    CreateMap<ClassDay, ClassDayViewModel>().PreserveReferences();
    CreateMap<StudentsClasses, StudentsClassesViewModel>().PreserveReferences();
    CreateMap<TeachersClasses, TeachersClassesViewModel>().PreserveReferences();

    CreateMap<Plan, PlanViewModel>().PreserveReferences();

    CreateMap<Role, RoleViewModel>().PreserveReferences();
    CreateMap<UsersRoles, UsersRolesViewModel>().PreserveReferences();

    CreateMap<Subscription, SubscriptionPreviewViewModel>().PreserveReferences();
    CreateMap<Subscription, SubscriptionProfileViewModel>().PreserveReferences();

    CreateMap<Tenant, TenantPreviewViewModel>()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address)).PreserveReferences();

    CreateMap<TenantPlan, TenantPlanViewModel>().PreserveReferences();
    CreateMap<Link, LinkViewModel>().PreserveReferences();

    CreateMap<Tenant, TenantProfileViewModel>()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address)).PreserveReferences();

    CreateMap<TimeTable, TimeTableViewModel>().PreserveReferences();
    CreateMap<ScheduleDay, ScheduleDayViewModel>().PreserveReferences();

    CreateMap<Invoice, InvoiceViewModel>().PreserveReferences();
    CreateMap<StripeCustomer, StripeCustomerViewModel>().PreserveReferences();
  }
}