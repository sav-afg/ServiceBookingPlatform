using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos;

namespace ServiceBookingPlatform.Services
{
    public class UserBookingService(AppDbContext Db) : IUserBookingService
    {
        private IQueryable<BookingDto> GetBookingQuery()
        {
            return Db.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Where(b => b.User != null && b.Service != null)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    ScheduledStart = b.ScheduledStart,
                    ScheduledEnd = b.ScheduledEnd,
                    Status = b.Status,
                    LastName = b.User!.LastName,
                    Email = b.User.Email,
                    ServiceName = b.Service!.ServiceName
                });
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            return await GetBookingQuery().ToListAsync();
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int bookingId)
        {
            return await GetBookingQuery()
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto newBooking)
        {
            var booking = new Booking
            {
                UserId = newBooking.UserId,
                ServiceId = newBooking.ServiceId,
                ScheduledStart = newBooking.ScheduledStart,
                ScheduledEnd = newBooking.ScheduledEnd,
                Status = newBooking.Status
            };

            Db.Bookings.Add(booking);
            await Db.SaveChangesAsync();

            // Fetch the complete DTO with user/service info
            return (await GetBookingByIdAsync(booking.Id))!;
        }

        public async Task<BookingDto?> UpdateBookingAsync(int bookingId, UpdateBookingDto updatedBooking)
        {
            var existingBooking = await Db.Bookings.FindAsync(bookingId);

            if (existingBooking == null)
            {
                return null;
            }

            existingBooking.ScheduledStart = updatedBooking.ScheduledStart;
            existingBooking.ScheduledEnd = updatedBooking.ScheduledEnd;
            existingBooking.Status = updatedBooking.Status;

            await Db.SaveChangesAsync();

            return await GetBookingByIdAsync(bookingId);
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var booking = await Db.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return false;
            }

            Db.Bookings.Remove(booking);
            await Db.SaveChangesAsync();
            return true;
        }
    }
}
