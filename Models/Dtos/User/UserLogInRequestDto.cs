using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserLogInRequestDto
    {
        [Required(ErrorMessage = "Email is required to log in")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required to log in")]
        public required string Password { get; set; }
    }
}
