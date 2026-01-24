using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos;

namespace ServiceBookingPlatform.Services
{
    public class UserBookingService(AppDbContext Db) : IUserBookingService
    {

        public async Task<Booking> CreateBookingAsync(Booking newBooking)
        {
            Db.Bookings.Add(newBooking);
            await Db.SaveChangesAsync();
            return newBooking;
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

        public async Task<List<BookingDto>> GetAllBookingsAsync()
            => await Db.Bookings
                .Include(b => b.User) // Explicitly load User navigation property
                .Where(b => b.User != null) // Filter out orphaned bookings
                .Select(b => new BookingDto
                {
                    ScheduledStart = b.ScheduledStart,
                    ScheduledEnd = b.ScheduledEnd,
                    Status = b.Status,
                    LastName = b.User!.LastName,
                    Email = b.User.Email
                }).ToListAsync();


        public async Task<BookingDto?> GetBookingByIdAsync(int bookingId)
        {
            var bookingDto = await Db.Bookings
                .Include(b => b.User) // Explicitly load User navigation property
                .Where(b => b.Id == bookingId && b.User != null)
                .Select(b => new BookingDto
                {
                    ScheduledStart = b.ScheduledStart,
                    ScheduledEnd = b.ScheduledEnd,
                    Status = b.Status,
                    LastName = b.User!.LastName,
                    Email = b.User.Email
                })
                .FirstOrDefaultAsync();

            return bookingDto;
        }



        public async Task<BookingDto?> UpdateBookingAsync(int bookingId, BookingDto updatedBooking)
        {
            var existingBooking = await Db.Bookings.FindAsync(bookingId);

            if (existingBooking == null)
            {
                return null;
            }

            // Update properties of the existing entity
            existingBooking.ScheduledStart = updatedBooking.ScheduledStart;
            existingBooking.ScheduledEnd = updatedBooking.ScheduledEnd;
            existingBooking.Status = updatedBooking.Status;
            existingBooking.LastName = updatedBooking.LastName;
            existingBooking.Email = updatedBooking.Email;

            await Db.SaveChangesAsync();

            var bookingDto = new BookingDto
            {
                ScheduledStart = existingBooking.ScheduledStart,
                ScheduledEnd = existingBooking.ScheduledEnd,
                Status = existingBooking.Status,
                LastName = existingBooking.LastName,
                Email = existingBooking.Email
            };

            return bookingDto;
        }
    }
}
