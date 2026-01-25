using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Role { get; set; }

    }
}
