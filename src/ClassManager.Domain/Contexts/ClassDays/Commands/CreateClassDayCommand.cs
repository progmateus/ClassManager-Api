using ClassManager.Domain.Shared.Commands;
using Flunt.Notifications;
using Flunt.Validations;

namespace ClassManager.Domain.Contexts.ClassDays.Commands
{
  public class CreateClassDayCommand : Notifiable, ICommand
  {
    public DateTime Date { get; set; }
    public string? HourStart { get; set; }
    public string? HourEnd { get; set; }
    public Guid ClassId { get; set; }

    public void Validate()
    {
      AddNotifications(new Contract()
      .Requires()
      .HasMinLen(HourStart, 3, "CreateClassDayCommand.HourStart", "HourStart min 3 characters")
      .HasMaxLen(HourStart, 10, "CreateClassDayCommand.HourStart", "HourStart max 40 characters")
      .HasMinLen(HourEnd, 3, "CreateClassDayCommand.HourEnd", "HourEnd min 3 characters")
      .HasMaxLen(HourEnd, 10, "CreateClassDayCommand.HourEnd", "HourEnd max 40 characters")
      .IsNotNull(ClassId, "CreateClassDayCommand.ClassId", "ClassId not null")
    );
    }
  }
}