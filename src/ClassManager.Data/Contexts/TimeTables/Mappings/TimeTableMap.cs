using ClassManager.Domain.Contexts.TimesTables.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.TimesTabless.Mappings;

public class TimeTableMap : IEntityTypeConfiguration<TimeTable>
{
  public void Configure(EntityTypeBuilder<TimeTable> builder)
  {
    builder.ToTable("TimesTables");

    builder.HasKey(x => x.Id);

    builder.HasOne(x => x.Tenant)
      .WithMany(p => p.TimesTables)
      .HasForeignKey(x => x.TenantId)
      .IsRequired(true);

    builder.Property(x => x.Name)
      .HasColumnName("Name")
      .HasColumnType("VARCHAR")
      .HasMaxLength(80)
      .IsRequired(true);
  }
}