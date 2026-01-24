using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string FirstName { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        public enum UserRole
        {
            Customer,
            Staff,
            Admin
        }

    }
}
