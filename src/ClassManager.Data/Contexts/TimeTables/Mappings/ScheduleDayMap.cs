using ClassManager.Domain.Contexts.ClassDays.Entities;
using ClassManager.Domain.Contexts.Shared.Enums;
using ClassManager.Domain.Contexts.TimesTables.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class ScheduleDayMap : IEntityTypeConfiguration<ScheduleDay>
{
  public void Configure(EntityTypeBuilder<ScheduleDay> builder)
  {
    builder.ToTable("SchedulesDays");

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

    builder.Property(x => x.HourEnd)
    .HasColumnName("HourEnd")
    .HasColumnType("VARCHAR")
    .HasMaxLength(10)
    .IsRequired(true);


    builder.Property(x => x.WeekDay)
    .IsRequired(true);

    builder.HasOne(x => x.TimeTable)
      .WithMany(p => p.SchedulesDays)
      .HasForeignKey(x => x.TimeTableId)
      .IsRequired(true);
  }
}