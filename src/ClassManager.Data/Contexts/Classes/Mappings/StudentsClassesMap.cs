using ClassManager.Domain.Contexts.Classes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class StudentsClassesMap : IEntityTypeConfiguration<StudentsClasses>
{
  public void Configure(EntityTypeBuilder<StudentsClasses> builder)
  {
    builder.ToTable("StudentsClasses");

    builder.HasKey(x => x.Id);

    builder.HasOne(e => e.User)
      .WithMany(u => u.StudentsClasses)
      .HasForeignKey("UserId")
      .HasPrincipalKey(u => u.Id)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(e => e.Class)
      .WithMany(c => c.StudentsClasses)
      .HasForeignKey("ClassId")
      .HasPrincipalKey(c => c.Id)
      .OnDelete(DeleteBehavior.Cascade);
  }
}