using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos;
namespace ServiceBookingPlatform.Services
{
    public interface IUserBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();

        Task<BookingDto?> GetBookingByIdAsync(int bookingId);

        Task<Booking> CreateBookingAsync(Booking newBooking);

        Task<BookingDto?> UpdateBookingAsync(int bookingId, BookingDto updatedBooking);

        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
