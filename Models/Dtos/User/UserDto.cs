using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserDto
    {
        [Required(ErrorMessage = "User must have a first name")]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "User must have a last name")]
        [StringLength(50, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "User must have an email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "User must have a password")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "User must have a phone number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "User must have a role")]
        public required string Role { get; set; }

    }
}
