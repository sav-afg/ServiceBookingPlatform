using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos.Booking;
namespace ServiceBookingPlatform.Services
{
    public interface IUserBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();

        Task<BookingDto?> GetBookingByIdAsync(int bookingId);

        Task<BookingDto> CreateBookingAsync(CreateBookingDto newBooking);

        Task<BookingDto?> UpdateBookingAsync(int bookingId, UpdateBookingDto updatedBooking);

        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
