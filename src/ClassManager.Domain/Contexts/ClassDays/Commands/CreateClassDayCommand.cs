using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.ClassDays.Commands
{
  public class CreateClassDayCommand : Notifiable, ICommand
  {
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? HourStart { get; set; }
    public string? HourEnd { get; set; }
    public Guid ClassId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(HourStart, 3, "Name", "Min 3 characters")
      .HasMaxLen(HourStart, 80, "Name", "Max 40 characters")
      .HasMinLen(HourStart, 3, "HourStart", "Min 3 characters")
      .HasMaxLen(HourStart, 10, "HourStart", "Max 40 characters")
      .HasMinLen(HourEnd, 3, "HourEnd", "Min 3 characters")
      .HasMaxLen(HourEnd, 10, "HourEnd", "Max 40 characters")
      .IsNotNull(ClassId, "ClassId", "not null")
    );
    }
  }
}