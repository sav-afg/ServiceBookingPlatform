using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Services.Common;

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

        public async Task<Result<BookingDto?>> CreateBookingAsync(CreateBookingDto newBooking)
        {
            if (newBooking.ScheduledEnd <= newBooking.ScheduledStart)
            {
                return Result<BookingDto?>.Failure("Scheduled end time must be after scheduled start time.");
            }

            if(newBooking.Status != "Pending" && newBooking.Status != "Confirmed" && newBooking.Status != "Cancelled" && newBooking.Status != "Completed")
            {
                return Result<BookingDto?>.Failure("Invalid booking status. Allowed values are: Pending, Confirmed, Cancelled, Completed.");
            }
            
            //var service = await Db.Services.FindAsync(newBooking.ServiceId);

            //Check to see if the service has been booked for the requested time
            bool bookingConflict = await Db.Bookings
                .AnyAsync(b => b.ServiceId == newBooking.ServiceId &&
                               ((newBooking.ScheduledStart >= b.ScheduledStart && newBooking.ScheduledStart < b.ScheduledEnd) ||
                                (newBooking.ScheduledEnd > b.ScheduledStart && newBooking.ScheduledEnd <= b.ScheduledEnd) ||
                                (newBooking.ScheduledStart <= b.ScheduledStart && newBooking.ScheduledEnd >= b.ScheduledEnd)));

            if(bookingConflict)
            {
                return Result<BookingDto?>.Failure("The service is already booked for the requested time.");
            }


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
            return Result<BookingDto?>.Success(
                await GetBookingByIdAsync(booking.Id),
                "Booking created successfully.");
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

        public async Task<bool> DeleteBookingAsync(int serviceId)
        {
            var service = await Db.Bookings.FindAsync(serviceId);

            if (service == null)
            {
                return false;
            }

            Db.Bookings.Remove(service);
            await Db.SaveChangesAsync();
            return true;
        }
    }
}
