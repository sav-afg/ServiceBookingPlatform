using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;

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

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await Db.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            var result = await Db.Bookings.FindAsync(bookingId);
            return await Task.FromResult(result);
        }

        public async Task<Booking?> UpdateBookingAsync(int bookingId, Booking updatedBooking)
        {
            var existingBooking = await Db.Bookings.FindAsync(bookingId);
            
            if (existingBooking == null)
            {
                return null;
            }
            
            // Update properties of the existing entity
            existingBooking.UserId = updatedBooking.UserId;
            existingBooking.ServiceId = updatedBooking.ServiceId;
            existingBooking.ScheduledStart = updatedBooking.ScheduledStart;
            existingBooking.ScheduledEnd = updatedBooking.ScheduledEnd;
            existingBooking.Status = updatedBooking.Status;
            
            await Db.SaveChangesAsync();
            return existingBooking;
        }
    }
}
