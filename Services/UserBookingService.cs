using Microsoft.AspNetCore.Http.HttpResults;
using ServiceBookingPlatform.Models;

namespace ServiceBookingPlatform.Services
{
    public class UserBookingService : IUserBookingService
    {

        static List<Booking> bookings =
        [
            new Booking { Id = 1, UserId = 101, ServiceId = 201, ScheduledStart = DateTime.Now, ScheduledEnd = DateTime.Now.AddMinutes(30), Status = "Pending" },
            new Booking { Id = 2, UserId = 102, ServiceId = 202, ScheduledStart = DateTime.Now, ScheduledEnd = DateTime.Now.AddMinutes(20), Status = "Confirmed" },
            new Booking { Id = 3, UserId = 103, ServiceId = 203, ScheduledStart = DateTime.Now, ScheduledEnd = DateTime.Now.AddMinutes(10), Status = "Cancelled" }
        ];
        public Task<Booking> CreateBookingAsync(Booking newBooking)
        {
            bookings.Add(newBooking);
            return Task.FromResult(newBooking);
        }

        public Task<bool> DeleteBookingAsync(int bookingId)
        {
            return bookings.Remove(bookings.First(b => b.Id == bookingId)) ? Task.FromResult(true) : Task.FromResult(false);

        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return bookings;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            var result = bookings.FirstOrDefault(b => b.Id == bookingId);
            return await Task.FromResult(result);
        }

        public Task<Booking> UpdateBookingAsync(int bookingId, Booking updatedBooking)
        {
            bookings.Remove(bookings.First(b => b.Id == bookingId));
            bookings.Add(updatedBooking);
            return Task.FromResult(updatedBooking);
        }
    }
}
