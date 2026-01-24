using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Services;

namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingController(IUserBookingService service) : ControllerBase
    {
        // GET: api/UserBooking
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetAllBookings()
        {
            return Ok(await service.GetAllBookingsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await service.GetBookingByIdAsync(id);
            return booking is null ? NotFound("Booking with the given ID was not found") : Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> AddBooking(Booking booking)
        {
            return Ok(await service.CreateBookingAsync(booking));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var result = await service.DeleteBookingAsync(id);
            return result ? Ok("Booking deleted successfully") : NotFound("Booking with the given ID was not found");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Booking>> UpdateBooking(int id, Booking booking)
        {
            var updatedBooking = await service.UpdateBookingAsync(id, booking);
            return Ok(updatedBooking);
        }
    }
}
