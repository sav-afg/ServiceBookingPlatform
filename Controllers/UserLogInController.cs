using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models.Dtos.User;
using ServiceBookingPlatform.Services;

namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogInController(IUserLogInService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> LogIn(UserLogInDto user)
        {
            var (success, message) = await service.ValidateUserCredentialsAsync(user);
            if (success)
            {
                return Ok(new { message });
            }
            else
            {
                return Unauthorized(new { message });
            }
        }

    }
}
