using ClassManager.Domain.Contexts.Classes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class ClassMap : IEntityTypeConfiguration<Class>
{
  public void Configure(EntityTypeBuilder<Class> builder)
  {
    builder.ToTable("Classes");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(80)
        .IsRequired(true);

    builder.Property(x => x.BusinessHour)
    .HasColumnName("BusinessHour")
    .HasColumnType("VARCHAR")
    .HasMaxLength(500)
    .IsRequired(false);

    builder.HasOne(x => x.Tenant)
        .WithMany(p => p.Classes)
        .HasForeignKey(x => x.TenantId)
        .IsRequired(true);
  }
}