using ClassManager.Domain.Contexts.Classes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassManager.Data.Contexts.Classes.Mappings;

public class TeachersClassesMap : IEntityTypeConfiguration<TeachersClasses>
{
  public void Configure(EntityTypeBuilder<TeachersClasses> builder)
  {
    builder.ToTable("TeachersClasses");

    builder.HasKey(x => x.Id);

    builder.HasOne(e => e.User)
      .WithMany(u => u.TeachersClasses)
      .HasForeignKey("UserId")
      .HasPrincipalKey(u => u.Id)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(e => e.Class)
      .WithMany(c => c.TeachersClasses)
      .HasForeignKey("ClassId")
      .HasPrincipalKey(c => c.Id)
      .OnDelete(DeleteBehavior.Cascade);
  }
}