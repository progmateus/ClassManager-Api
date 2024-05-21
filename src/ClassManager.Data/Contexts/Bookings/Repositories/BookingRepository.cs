using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;

namespace ClassManager.Data.Contexts.Bookings.Repositories;


public class BookingRepository : Repository<Booking>, IBookingRepository
{
  public BookingRepository(AppDbContext dbContext) : base(dbContext) { }
}