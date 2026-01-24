using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Models.Dtos;
using ServiceBookingPlatform.Services;

namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingController(IUserBookingService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAllBookings()
        {
            var bookings = await service.GetAllBookingsAsync();

            if (bookings.Count == 0)
            {
                return NotFound("No bookings found.");
            }

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            var booking = await service.GetBookingByIdAsync(id);
            return booking is null 
                ? NotFound($"Booking with ID {id} was not found") 
                : Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> AddBooking(CreateBookingDto booking)
        {
            var createdBooking = await service.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(int id, UpdateBookingDto booking)
        {
            var updatedBooking = await service.UpdateBookingAsync(id, booking);
            return updatedBooking is null 
                ? NotFound($"Booking with ID {id} was not found") 
                : Ok(updatedBooking);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var result = await service.DeleteBookingAsync(id);
            return result 
                ? Ok($"Booking with ID {id} successfully deleted.") 
                : NotFound($"Booking with ID {id} was not found");
        }
    }
}
