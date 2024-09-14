using ClasManager.Domain.Contexts.Bookings.Entities;

namespace ClassManager.Domain.Contexts.Classes.ViewModels
{
  public class ClassDayViewModel
  {
    public DateTime Date { get; private set; }
    public string? HourStart { get; private set; }
    public string? HourEnd { get; private set; }
    public int Status { get; private set; }
    public string? Observation { get; private set; }
    public Guid ClassId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ClassViewModel Class { get; private set; } = null!;
    public List<Booking> Bookings { get; private set; } = [];
  }
}