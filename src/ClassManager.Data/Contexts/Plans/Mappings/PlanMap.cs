using ClassManager.Domain.Contexts.Plans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Plans.Mappings;

public class PlanMap : IEntityTypeConfiguration<Plan>
{
  public void Configure(EntityTypeBuilder<Plan> builder)
  {
    builder.ToTable("Plans");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(80)
        .IsRequired(true);

    builder.Property(x => x.Description)
        .HasColumnName("Description")
        .HasColumnType("VARCHAR")
        .HasMaxLength(300)
        .IsRequired(false);

    builder.Property(x => x.ClassesLimit)
    .HasColumnName("ClassesLimit")
    .HasColumnType("TINYINT")
    .IsRequired(true);


    builder.Property(x => x.StudentsLimit)
    .HasColumnName("StudentsLimit")
    .HasColumnType("INT")
    .IsRequired(true);

    builder.Property(x => x.Price)
    .HasColumnName("Price")
    .HasColumnType("DECIMAL")
    .IsRequired(true);

  }
}