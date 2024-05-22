using ClasManager.Domain.Contexts.Bookings.Entities;
using ClassManager.Data.Contexts.shared.Repositories;
using ClassManager.Data.Data;
using ClassManager.Domain.Contexts.Bookings.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ClassManager.Data.Contexts.Bookings.Repositories;


public class BookingRepository : Repository<Booking>, IBookingRepository
{
  public BookingRepository(AppDbContext dbContext) : base(dbContext) { }

  public async Task<Booking?> GetByUserIdAndClassDayId(Guid userId, Guid classDayId)
  {
    return await DbSet.Include(x => x.ClassDay).FirstOrDefaultAsync(x => x.UserId == userId && x.ClassDayId == classDayId);
  }

  public async Task<Booking?> GetWithInclude(Guid userId, Guid bookingId)
  {
    return await DbSet.Include(x => x.ClassDay).FirstOrDefaultAsync(x => x.UserId == userId && x.Id == bookingId);
  }
}