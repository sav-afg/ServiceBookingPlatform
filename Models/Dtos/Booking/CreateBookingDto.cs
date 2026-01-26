using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.Booking
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Booking must be associated with a user.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Booking must be associated with a service.")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Booking must have a scheduled start time.")]
        public DateTime ScheduledStart { get; set; }

        [Required(ErrorMessage = "Booking must have a scheduled end time.")]
        public DateTime ScheduledEnd { get; set; }

        [Required(ErrorMessage = "Booking status is required.")]
        public required string Status { get; set; }
    }
}
