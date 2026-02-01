using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /* Only authenticated users can access these endpoints
     * Customers can manage their own bookings
     * Staff can view all bookings and manage bookings assigned to them
     * Admins have full access to all bookings
     */

    [Authorize]
    public class UserBookingController(IUserBookingService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<ActionResult<List<BookingDto>>> GetAllBookings()
        {
            var bookings = await service.GetAllBookingsAsync(User);

            if (bookings.Count == 0)
            {
                return NotFound("No bookings found.");
            }

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            try
            {
                var booking = await service.GetBookingByIdAsync(id, User);
                return booking is null
                    ? NotFound($"Booking with ID {id} was not found")
                    : Ok(booking);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> AddBooking(CreateBookingDto booking)
        {

            var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.NameId)!.Value);

            var result = await service.CreateBookingAsync(userId, booking);

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
            var result = await service.UpdateBookingAsync(id, booking, User);

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
            try
            {
                var result = await service.DeleteBookingAsync(id, User);
                return result
                    ? Ok($"Booking with ID {id} successfully deleted.")
                    : NotFound($"Booking with ID {id} was not found");

            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
