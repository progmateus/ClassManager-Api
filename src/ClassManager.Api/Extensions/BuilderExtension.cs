using System.Text;
using System.Text.Json.Serialization;
using ClasManager.Domain.Contexts.Bookings.Handlers;
using ClasManager.Domain.Contexts.ClassDays.Jobs.GenerateMonthlyClassesDays;
using classManager.Data.Contexts.Roles.Repositories;
using classManager.Data.Contexts.Subscriptions.Repositories;
using ClassManager.Data.Contexts.Accounts.Repositories;
using ClassManager.Data.Contexts.Accounts.Services;
using ClassManager.Data.Contexts.Addresses.Repositories;
using ClassManager.Data.Contexts.Bookings.Repositories;
using ClassManager.Data.Contexts.Plans.Repositories;
using ClassManager.Data.Contexts.Tenants.Repositories;
using ClassManager.Data.Contexts.Tenants.Services;
using ClassManager.Data.Data;
using ClassManager.Domain;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Addresses.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Services;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using ClassManager.Domain.Contexts.ClassDays.Handlers;
using ClassManager.Domain.Contexts.ClassDays.Helpers;
using ClassManager.Domain.Contexts.ClassDays.Repositories.Contracts;
using ClassManager.Domain.Contexts.Classes.Handlers;
using ClassManager.Domain.Contexts.Classes.Repositories.Contracts;
using ClassManager.Domain.Contexts.Invoices.Handlers;
using ClassManager.Domain.Contexts.Invoices.Repositories.Contracts;
using ClassManager.Domain.Contexts.Plans.Handlers;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Subscriptions.Handlers;
using ClassManager.Domain.Contexts.Subscriptions.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Contexts.TimesTables.Commands;
using ClassManager.Domain.Contexts.TimesTables.Handlers;
using ClassManager.Domain.Contexts.TimesTabless.Handlers;
using ClassManager.Domain.Contexts.Usernames.Handlers;
using ClassManager.Domain.Libs.MassTransit.Events;
using ClassManager.Domain.Libs.MassTransit.Publish;
using ClassManager.Domain.Services;
using ClassManager.Domain.Services.Stripe.Handlers;
using ClassManager.Domain.Services.Stripe.Repositories.Contracts;
using ClassManager.Domain.Shared.Services.AccessControlService;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace ClassManager.Api.Extensions;
public static class BuilderExtension
{
  public static void AddConfiguration(this WebApplicationBuilder builder)
  {
    Configuration.Database.ConnectionString =
        builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

    Configuration.Secrets.ApiKey =
        builder.Configuration.GetSection("Secrets").GetValue<string>("ApiKey") ?? string.Empty;
    Configuration.Secrets.JwtPrivateKey =
        builder.Configuration.GetSection("Secrets").GetValue<string>("JwtPrivateKey") ?? string.Empty;
    Configuration.Secrets.PasswordSaltKey =
        builder.Configuration.GetSection("Secrets").GetValue<string>("PasswordSaltKey") ?? string.Empty;

    Configuration.SendGrid.ApiKey =
        builder.Configuration.GetSection("SendGrid").GetValue<string>("ApiKey") ?? string.Empty;

    Configuration.Email.DefaultFromName =
        builder.Configuration.GetSection("Email").GetValue<string>("DefaultFromName") ?? string.Empty;
    Configuration.Email.DefaultFromEmail =
        builder.Configuration.GetSection("Email").GetValue<string>("DefaultFromEmail") ?? string.Empty;

    Configuration.Stripe.ApiKey =
    builder.Configuration.GetSection("Stripe").GetValue<string>("ApiKey") ?? string.Empty;

    Configuration.Stripe.PublishableKey =
    builder.Configuration.GetSection("Stripe").GetValue<string>("PublishableKey") ?? string.Empty;

    Configuration.Stripe.SecretKey =
    builder.Configuration.GetSection("Stripe").GetValue<string>("SecretKey") ?? string.Empty;
  }

  public static void AddDatabase(this WebApplicationBuilder builder)
  {
    builder.Services.AddDbContext<AppDbContext>(x =>
        x.UseSqlServer(
            Configuration.Database.ConnectionString
        ));
  }

  public static void AddJwtAuthentication(this WebApplicationBuilder builder)
  {
    builder.Services
        .AddAuthentication(x =>
        {
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.TokenValidationParameters = new TokenValidationParameters
          {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        });
    builder.Services.AddAuthorization(x => { x.AddPolicy("Admin", p => p.RequireRole("admin")); });
  }

  public static void AddMediator(this WebApplicationBuilder builder)
  {
    builder.Services.AddMediatR(x
        => x.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
  }

  public static void AddControllers(this WebApplicationBuilder builder)
  {
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
      options.SuppressModelStateInvalidFilter = true;
    }).AddJsonOptions(x =>
    {
      x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    });
  }

  public static void AddRabbitMQService(this WebApplicationBuilder builder)
  {
    builder.Services.AddMassTransit(bussConfigurator =>
    {
      bussConfigurator.AddConsumer<GeneratedClassesDaysEventConsumer>();
      bussConfigurator.AddConsumer<RefreshClassClassesDaysEventConsumer>();

      bussConfigurator.UsingRabbitMq((ctx, config) =>
      {
        config.Host(new Uri(Configuration.RabbitMq.Uri), host =>
        {
          host.Username(Configuration.RabbitMq.Username);
          host.Password(Configuration.RabbitMq.Password);
        });

        config.ConfigureEndpoints(ctx);
      });
    });
  }

  public static void AddQuartz(this WebApplicationBuilder builder)
  {
    builder.Services.AddQuartz(opt =>
    {
      opt.UseMicrosoftDependencyInjectionJobFactory();
      var generateMonthlyClassesDaysJobKey = new JobKey("GenerateMonthlyClassesDaysJob");
      opt.AddJob<GenerateMonthlyClassesDaysJob>(options => options.WithIdentity(generateMonthlyClassesDaysJobKey));
      opt.AddTrigger(options =>
      {
        options
        .ForJob(generateMonthlyClassesDaysJobKey)
        .WithIdentity("GenerateMonthlyClassesDaysJob-trigger")
        .WithCronSchedule(builder.Configuration.GetSection("GenerateMonthlyClassesDaysJob:CronSchedule").Value ?? "0 0 4 28 * ? *");
      });
    });

    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
  }

  public static void AddServices(this WebApplicationBuilder builder)
  {

    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);
    builder.Services.Configure<GenerateMonthlyClassesDaysOptions>(builder.Configuration.GetSection(GenerateMonthlyClassesDaysOptions.GenerateMonthlyClassesDaysOptionsKey));

    builder.Services.AddTransient<
        IUserRepository,
        UserRepository>();

    builder.Services.AddTransient<
        IEmailService,
        EmailService>();


    builder.Services.AddTransient<
      ITenantRepository,
      TenantRepository>();

    builder.Services.AddTransient<
      IPlanRepository,
      PlanRepository>();

    builder.Services.AddTransient<
      IRoleRepository,
      RoleRepository>();

    builder.Services.AddTransient<
    IUsersRolesRepository,
    UsersRolesRepository>();

    builder.Services.AddTransient<
    ITenantPlanRepository,
    TenantPlanRepository>();

    builder.Services.AddTransient<
    IClassRepository,
    ClassRepository>();

    builder.Services.AddTransient<
    ITeacherClassesRepository,
    TeachersClassesRepository>();

    builder.Services.AddTransient<
    IStudentsClassesRepository,
    StudentsClassesRepository>();

    builder.Services.AddTransient<
    ISubscriptionRepository,
    SubscriptionRepository>();

    builder.Services.AddTransient<
    IClassDayRepository,
    ClassDayRepository>();


    builder.Services.AddTransient<
    IBookingRepository,
    BookingRepository>();

    builder.Services.AddTransient<
    ITimeTableRepository,
    TimeTableRepository>();

    builder.Services.AddTransient<
    IScheduleDayRepository,
    ScheduleDayRepository>();

    builder.Services.AddTransient<
    IAccessControlService,
    AccesControlService>();

    builder.Services.AddTransient<
    IPublishBus,
    PublishBus>();

    builder.Services.AddTransient<
    ILinkRepository,
    LinkRepository>();

    builder.Services.AddTransient<
    IInvoiceRepository,
    InvoiceRepository>();

    builder.Services.AddTransient<
    IPaymentService,
    PaymentService>();

    builder.Services.AddTransient<
    IStripeCustomerRepository,
    StripeCustomerRepository>();

    builder.Services.AddTransient<
    IAddressRepository,
    AddressRepository>();

    builder.Services.AddTransient<
    IImageRepository,
    ImageRepository>();

    builder.Services.AddTransient<TokenService>();

    builder.Services.AddTransient<AuthHandler>();

    builder.Services.AddTransient<CreateUserHandler>();
    builder.Services.AddTransient<ListUsersHandler>();
    builder.Services.AddTransient<UpdateUserHandler>();
    builder.Services.AddTransient<DeleteUserHandler>();
    builder.Services.AddTransient<GetUserProfileHandler>();
    builder.Services.AddTransient<GetUserByUsernameHandler>();
    builder.Services.AddTransient<UploadUserAvatarHandler>();

    builder.Services.AddTransient<CreateTenantHandler>();
    builder.Services.AddTransient<ListTenantsHandler>();
    builder.Services.AddTransient<UpdateTenantHandler>();
    builder.Services.AddTransient<DeleteTenantHandler>();
    builder.Services.AddTransient<GetTenantHandler>();
    builder.Services.AddTransient<CreateTenantBankAccountHandler>();
    builder.Services.AddTransient<GetTenantProfileHandler>();
    builder.Services.AddTransient<RefreshTenantSubscriptionHandler>();
    builder.Services.AddTransient<UploadTenantAvatarHandler>();
    builder.Services.AddTransient<CreateImageHandler>();
    builder.Services.AddTransient<DeleteImageHandler>();


    builder.Services.AddTransient<CreatePlandHandler>();
    builder.Services.AddTransient<ListPlansHandler>();
    builder.Services.AddTransient<UpdatePlandHandler>();
    builder.Services.AddTransient<DeletePlanHandler>();


    builder.Services.AddTransient<CreateRoleHandler>();
    builder.Services.AddTransient<ListRolesHandler>();
    builder.Services.AddTransient<UpdateRoleHandler>();
    builder.Services.AddTransient<DeleteRoleHandler>();

    builder.Services.AddTransient<UpdateUsersRolesHandler>();
    builder.Services.AddTransient<GetUserRolesHandler>();
    builder.Services.AddTransient<ListUsersRolesHandler>();
    builder.Services.AddTransient<DeleteUserRoleHandler>();
    builder.Services.AddTransient<CreateUserRoleHandler>();


    builder.Services.AddTransient<CreateTenantPlanHandler>();
    builder.Services.AddTransient<UpdateTenantPlanHandler>();
    builder.Services.AddTransient<ListTenantPlansHandler>();
    builder.Services.AddTransient<GetTenantPlanByIdHandler>();
    builder.Services.AddTransient<DeleteTenantPlanHandler>();

    builder.Services.AddTransient<CreateClassHandler>();
    builder.Services.AddTransient<UpdateClassHandler>();
    builder.Services.AddTransient<ListClassesHandler>();
    builder.Services.AddTransient<GetClassByIdHandler>();
    builder.Services.AddTransient<DeleteClassHandler>();
    builder.Services.AddTransient<GetClassProfileHandler>();
    builder.Services.AddTransient<ListStudentsByClassHandler>();
    builder.Services.AddTransient<ListTeachersByClassHandler>();
    builder.Services.AddTransient<UpdateClassTimeTableHandler>();

    builder.Services.AddTransient<UpdateTeacherClassHandler>();
    builder.Services.AddTransient<RemoveTeacherFromClassHandler>();
    builder.Services.AddTransient<ListTeacherClassesHandler>();

    builder.Services.AddTransient<UpdateOneStudentClassHandler>();
    builder.Services.AddTransient<UpdateManyStudentsClassesHandler>();
    builder.Services.AddTransient<RemoveStudentFromClassHandler>();
    builder.Services.AddTransient<ListStudentClassesHandler>();
    builder.Services.AddTransient<TransferClassStudentsHandler>();

    builder.Services.AddTransient<CreateSubscriptionHandler>();
    builder.Services.AddTransient<UpdateSubscriptionStatusHandler>();
    builder.Services.AddTransient<UpdateSubscriptionPlanHandler>();
    builder.Services.AddTransient<ListSubscriptionsHandler>();
    builder.Services.AddTransient<DeleteSubscriptionHandler>();
    builder.Services.AddTransient<GetSubscriptionProfileHandler>();

    builder.Services.AddTransient<CreateClassDayHandler>();
    builder.Services.AddTransient<UpdateClassDayHandler>();
    builder.Services.AddTransient<GetClassDayByIdHandler>();
    builder.Services.AddTransient<ListClassesDaysHandler>();
    builder.Services.AddTransient<DeleteClassDayHandler>();
    builder.Services.AddTransient<GenerateClassesDaysHandler>();



    builder.Services.AddTransient<CreateBookingHandler>();
    builder.Services.AddTransient<ListUserBookingsHandler>();
    builder.Services.AddTransient<ListClassDayBookingsHandler>();
    builder.Services.AddTransient<DeleteBookingHandler>();


    builder.Services.AddTransient<VerifyUsernameHandler>();
    builder.Services.AddTransient<GenerateClassesDaysHelper>();

    builder.Services.AddTransient<CreateTimeTableHandler>();
    builder.Services.AddTransient<UpdateTimetableHandler>();
    builder.Services.AddTransient<ListTimesTablesHandler>();
    builder.Services.AddTransient<GetTimeTableHandler>();

    builder.Services.AddTransient<CreateInvoiceHandler>();
    builder.Services.AddTransient<ListInvoicesHandler>();
    builder.Services.AddTransient<UpdateInvoiceStatusHandler>();
    builder.Services.AddTransient<DeleteInvoiceHandler>();

    builder.Services.AddTransient<CreateStripeWebhookHandler>();
    builder.Services.AddTransient<UpdateStripeInvoiceWebhookHandler>();
    builder.Services.AddTransient<CreateStripeSubscriptionWebhookHandler>();
    builder.Services.AddTransient<UpdateStripeAccountWebhookHandler>();
    builder.Services.AddTransient<UpdateStripeAccountWebhookHandler>();
    builder.Services.AddTransient<FinalizeStripeInvoiceWebhookHandler>();
    builder.Services.AddTransient<UpdateStripeSubscriptionWebhookHandler>();

    builder.Services.AddTransient<CreateTenantAddressHandler>();
    builder.Services.AddTransient<UpdateUserAddressHandler>();
    builder.Services.AddTransient<UpdateClassAddressHandler>();
    builder.Services.AddTransient<DeleteAddressHandler>();
    builder.Services.AddTransient<ListTenantAddressesHandler>();
  }
}