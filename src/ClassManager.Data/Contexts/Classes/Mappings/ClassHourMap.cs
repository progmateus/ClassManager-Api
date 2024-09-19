using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class ClassHourMap : IEntityTypeConfiguration<ClassHour>
{
  public void Configure(EntityTypeBuilder<ClassHour> builder)
  {
    builder.ToTable("ClassesHours");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.HourStart)
    .HasColumnName("HourStart")
    .HasColumnType("VARCHAR")
    .HasMaxLength(10)
    .IsRequired(true);

    builder.Property(x => x.HourEnd)
    .HasColumnName("HourEnd")
    .HasColumnType("VARCHAR")
    .HasMaxLength(10)
    .IsRequired(true);

    builder.Property(x => x.WeekDay)
    .IsRequired(true);
  }
}