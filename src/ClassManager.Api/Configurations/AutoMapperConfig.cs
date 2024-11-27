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


    CreateMap<Class, ClassViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<ClassDay, ClassDayViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<StudentsClasses, StudentsClassesViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<TeachersClasses, TeachersClassesViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Plan, PlanViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Role, RoleViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<UsersRoles, UsersRolesPreviewViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<UsersRoles, UsersRolesProfileViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Subscription, SubscriptionPreviewViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<Subscription, SubscriptionProfileViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Tenant, TenantPreviewViewModel>()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address)).PreserveReferences().MaxDepth(1);

    CreateMap<TenantPlan, TenantPlanViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<Link, LinkViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Tenant, TenantProfileViewModel>()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address)).PreserveReferences().MaxDepth(1);

    CreateMap<TimeTable, TimeTableViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<ScheduleDay, ScheduleDayViewModel>().PreserveReferences().MaxDepth(1);

    CreateMap<Invoice, InvoiceViewModel>().PreserveReferences().MaxDepth(1);
    CreateMap<StripeCustomer, StripeCustomerViewModel>().PreserveReferences().MaxDepth(1);
  }
}