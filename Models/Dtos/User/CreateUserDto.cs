using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class CreateUserDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        public required string FirstName { get; set; }
        
        public string Role { get; set; } = "Customer";

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
