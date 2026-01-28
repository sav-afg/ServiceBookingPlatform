using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Services;

namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var result = await service.CreateBookingAsync(booking);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetBookingById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BookingDto>> UpdateBooking(int id, UpdateBookingDto booking)
        {
            var result = await service.UpdateBookingAsync(id, booking);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(result.Data);

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
