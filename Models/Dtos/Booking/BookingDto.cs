using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.Booking
{
    public class BookingDto
    {
        [Required(ErrorMessage = "Booking must have a unique identifier")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Booking must have a scheduled start time.")]
        public DateTime ScheduledStart { get; set; }

        [Required(ErrorMessage = "Booking must have a scheduled end time.")]
        public DateTime ScheduledEnd { get; set; }

        [Required(ErrorMessage = "Booking status is required.")]
        public required string Status { get; set; }

        // Denormalized user details for simpler identification
        [Required(ErrorMessage = "Booking must include user last name.")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Booking must include user email.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Booking must include service name.")]
        public required string ServiceName { get; set; }
    }
}
