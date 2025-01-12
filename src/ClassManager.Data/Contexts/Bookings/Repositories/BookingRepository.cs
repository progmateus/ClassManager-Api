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
    return await DbSet.Include(x => x.ClassDay).Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId && x.Id == bookingId);
  }

  public async Task<List<Booking>> ListByUserIdAndTenantId(Guid? tenantId, Guid userId)
  {
    return await DbSet
    .Include(x => x.ClassDay)
    .ThenInclude(x => x.Class)
    .Where(x => x.UserId == userId && (!tenantId.HasValue || x.ClassDay.Class.TenantId == tenantId.Value))
    .ToListAsync();
  }

  public async Task<List<Booking>> ListByUserIdOrTenantIdOrClassDayIdWithPagination(Guid? tenantId, Guid? userId, Guid? classDayId, string search = "", int skip = 0, int limit = int.MaxValue, CancellationToken cancellationToken = default)
  {
    return await DbSet
      .Include(x => x.ClassDay)
      .ThenInclude(x => x.Class)
      .Where(x => !userId.HasValue || x.UserId == userId)
      .Where(x => !classDayId.HasValue || x.ClassDayId == classDayId)
      .Where(x => !tenantId.HasValue || x.ClassDay.Class.TenantId == tenantId)
      .Skip(skip)
      .Take(limit)
      .ToListAsync();
  }

  public async Task<int> CountByClassDay(Guid tenantId, Guid classDayId, CancellationToken cancellationToken = default)
  {
    return await DbSet
      .Include(x => x.ClassDay)
      .ThenInclude(x => x.Class)
      .Where(x => x.ClassDayId == classDayId)
      .Where(x => x.ClassDay.Class.TenantId == tenantId)
      .CountAsync(cancellationToken);
  }
}