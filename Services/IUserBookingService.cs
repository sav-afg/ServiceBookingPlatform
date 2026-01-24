using ServiceBookingPlatform.Models;

namespace ServiceBookingPlatform.Services
{
    public interface IUserBookingService
    {
        Task<List<Booking>> GetAllBookingsAsync();

        Task<Booking?> GetBookingByIdAsync(int bookingId);

        Task<Booking> CreateBookingAsync(Booking newBooking);

        Task<Booking?> UpdateBookingAsync(int bookingId, Booking updatedBooking);

        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
