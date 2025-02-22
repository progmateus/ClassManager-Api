using ClassManager.Domain.Contexts.Bookings.ViewModels;
using ClassManager.Domain.Contexts.Classes.ViewModels;
using ClassManager.Domain.Contexts.Shared.Enums;

namespace ClassManager.Domain.Contexts.ClassDays.ViewModels
{
  public class ClassDayViewModel
  {
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Name { get; set; }
    public string? HourStart { get; set; }
    public string? HourEnd { get; set; }
    public EClassDayStatus Status { get; set; }
    public string? Observation { get; set; }
    public int BookingsCount { get; set; } = 0;
    public Guid ClassId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ClassViewModel? Class { get; set; }
    public IList<BookingViewModel> Bookings { get; set; } = [];

  }
}