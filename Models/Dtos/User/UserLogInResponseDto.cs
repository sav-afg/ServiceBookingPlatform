using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserLogInResponseDto
    {
        [Required(ErrorMessage = "Email is required in the login response")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Access token is required in the login response")]
        public required string AccessToken { get; set; }

        [Required(ErrorMessage = "ExpiresIn time is required in the login response")]
        public int ExpiresIn { get; set; }

    }
}
