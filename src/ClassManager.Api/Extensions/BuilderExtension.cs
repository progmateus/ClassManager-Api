using System.Text;
using classManager.Data.Contexts.Roles.Repositories;
using ClassManager.Data.Contexts.Accounts.Repositories;
using ClassManager.Data.Contexts.Accounts.Services;
using ClassManager.Data.Contexts.Plans.Repositories;
using ClassManager.Data.Contexts.Tenants.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain;
using ClassManager.Domain.Contexts.Accounts.Handlers;
using ClassManager.Domain.Contexts.Accounts.Repositories.Contracts;
using ClassManager.Domain.Contexts.Auth.Services;
using ClassManager.Domain.Contexts.Plans.Handlers;
using ClassManager.Domain.Contexts.Plans.Repositories;
using ClassManager.Domain.Contexts.Roles.Commands;
using ClassManager.Domain.Contexts.Roles.Handlers;
using ClassManager.Domain.Contexts.Roles.Repositories.Contracts;
using ClassManager.Domain.Contexts.Tenants.Handlers;
using ClassManager.Domain.Contexts.Tenants.Repositories.Contracts;
using ClassManager.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
    builder.Services.AddControllers();
  }

  public static void AddServices(this WebApplicationBuilder builder)
  {
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

    builder.Services.AddTransient<TokenService>();

    builder.Services.AddTransient<CreateUserHandler>();
    builder.Services.AddTransient<ListUsersHandler>();
    builder.Services.AddTransient<UpdateUserHandler>();
    builder.Services.AddTransient<DeleteUserHandler>();


    builder.Services.AddTransient<CreateTenantHandler>();
    builder.Services.AddTransient<ListTenantsHandler>();
    builder.Services.AddTransient<UpdateTenantHandler>();
    builder.Services.AddTransient<DeleteTenantHandler>();


    builder.Services.AddTransient<CreatePlandHandler>();
    builder.Services.AddTransient<ListPlansHandler>();
    builder.Services.AddTransient<UpdatePlandHandler>();
    builder.Services.AddTransient<DeletePlanHandler>();


    builder.Services.AddTransient<CreateRoleHandler>();
    builder.Services.AddTransient<ListRolesHandler>();
    builder.Services.AddTransient<UpdateRoleHandler>();
    builder.Services.AddTransient<DeleteroleHandler>();

  }
}