namespace ServiceBookingPlatform.Models.Dtos
{
    public class BookingDto
    {
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public required string Status { get; set; }

        // Denormalized user details for simpler identification
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public required string ServiceName { get; set; }
    }
}
