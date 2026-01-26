using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "Customer";

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password hash is required")]
        public required string PasswordHash { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public required string PhoneNumber { get; set; }
    }
}
