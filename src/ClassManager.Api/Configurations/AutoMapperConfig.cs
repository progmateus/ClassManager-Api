using AutoMapper;
using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.Addresses.Entites;
using ClassManager.Domain.Contexts.Addresses.ViewModels;
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
    CreateMap<User, UserViewModel>()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document.Number));

    CreateMap<Booking, BookingViewModel>();


    CreateMap<Class, ClassViewModel>();
    CreateMap<Class, ClassPreviewViewModel>();
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
    CreateMap<Link, LinkViewModel>();
    CreateMap<Image, ImageViewModel>();

    CreateMap<TimeTable, TimeTableViewModel>();
    CreateMap<ScheduleDay, ScheduleDayViewModel>();

    CreateMap<Invoice, InvoiceViewModel>();
    CreateMap<StripeCustomer, StripeCustomerViewModel>();
    CreateMap<Address, AddressViewModel>();
  }
}