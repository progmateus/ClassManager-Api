using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Data.Contexts.Accounts.Mappings;
using ClassManager.Data.Contexts.Bookings.Mappings;
using ClassManager.Data.Contexts.Classes.Mappings;
using ClassManager.Data.Contexts.Plans.Mappings;
using ClassManager.Data.Contexts.Tenants.Mappings;
using ClassManager.Data.Contexts.TimeTables.Mappings;
using ClassManager.Domain.Contexts.Accounts.Entities;
using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Plans.Entities;
using ClassManager.Domain.Contexts.Roles.Entities;
using ClassManager.Domain.Contexts.Subscriptions.Entities;
using ClassManager.Domain.Contexts.Tenants.Entities;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    ChangeTracker.AutoDetectChangesEnabled = false;
  }

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Tenant> Tenants { get; set; } = null!;
  public DbSet<Plan> Plans { get; set; } = null!;
  public DbSet<Role> Roles { get; set; } = null!;
  public DbSet<Class> Classes { get; set; } = null!;
  public DbSet<UsersRoles> UsersRoles { get; set; } = null!;
  public DbSet<TenantPlan> TenantPlans { get; set; } = null!;
  public DbSet<TeachersClasses> TeachersClasses { get; set; } = null!;
  public DbSet<StudentsClasses> StudentsClasses { get; set; } = null!;
  public DbSet<Subscription> Subscriptions { get; set; } = null!;
  public DbSet<ClassDay> ClassDays { get; set; } = null!;
  public DbSet<Booking> Bookings { get; set; } = null!;
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    // se a propriedade for string ou vazia vai virar varchar(100)
    modelBuilder.Ignore<Notification>();

    foreach (var property in modelBuilder.Model.GetEntityTypes()
      .SelectMany(e => e.GetProperties()
        .Where(p => p.ClrType == typeof(string))))
      property.SetColumnType("varchar(100)");

    // qnd deletar pai, seta o relacionamento para null
    foreach (var relationship in modelBuilder.Model.GetEntityTypes()
      .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

    // aplica todas as entidades para n precisar ir uma por uma
    /* modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly); */

    modelBuilder.ApplyConfiguration(new UserMap());
    modelBuilder.ApplyConfiguration(new TenantMap());
    modelBuilder.ApplyConfiguration(new PlanMap());
    modelBuilder.ApplyConfiguration(new RoleMap());
    modelBuilder.ApplyConfiguration(new UsersRolesMap());
    modelBuilder.ApplyConfiguration(new TenantPlanMap());
    modelBuilder.ApplyConfiguration(new ClassMap());
    modelBuilder.ApplyConfiguration(new TeachersClassesMap());
    modelBuilder.ApplyConfiguration(new StudentsClassesMap());
    modelBuilder.ApplyConfiguration(new SubscriptionMap());
    modelBuilder.ApplyConfiguration(new ClassDayMap());
    modelBuilder.ApplyConfiguration(new BookingMap());
    modelBuilder.ApplyConfiguration(new TimeTableMap());

    base.OnModelCreating(modelBuilder);
  }


  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
    {
      if (entry.State == EntityState.Added)
      {
        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
      }

      if (entry.State == EntityState.Modified)
      {
        entry.Property("CreatedAt").IsModified = false;
      }
    }
    return base.SaveChangesAsync(cancellationToken);
  }
}