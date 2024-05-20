using ClassManager.Domain.Contexts.Classes.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class ClassDayMap : IEntityTypeConfiguration<ClassDay>
{
  public void Configure(EntityTypeBuilder<ClassDay> builder)
  {
    builder.ToTable("ClassDays");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Date)
      .IsRequired(true);

    builder.Property(x => x.HourStart)
      .HasColumnName("HourStart")
      .HasColumnType("VARCHAR")
      .HasMaxLength(10)
      .IsRequired(false);

    builder.Property(x => x.HourEnd)
      .HasColumnName("HourEnd")
      .HasColumnType("VARCHAR")
      .HasMaxLength(10)
      .IsRequired(false);

    builder.Property(x => x.Status)
      .HasColumnName("Status")
      .HasColumnType("TINYINT")
      .HasDefaultValue(EClassDayStatus.PENDING);

    builder.Property(x => x.Observation)
      .HasColumnName("Observation")
      .HasColumnType("VARCHAR")
      .HasMaxLength(200)
      .IsRequired(false);

    builder.HasOne(x => x.Class)
      .WithMany(p => p.ClassDays)
      .HasForeignKey(x => x.ClassId)
      .IsRequired(true);
  }
}