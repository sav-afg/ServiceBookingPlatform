using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Services.Common;
namespace ServiceBookingPlatform.Services
{
    public interface IUserBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();

        Task<BookingDto?> GetBookingByIdAsync(int bookingId);

        Task<Result<BookingDto?>> CreateBookingAsync(CreateBookingDto newBooking);

        Task<Result<BookingDto?>> UpdateBookingAsync(int bookingId, UpdateBookingDto updatedBooking);

        Task<bool> DeleteBookingAsync(int bookingId);
    }
}
