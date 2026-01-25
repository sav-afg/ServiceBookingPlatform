namespace ServiceBookingPlatform.Models.Dtos.Booking
{
    public class UpdateBookingDto
    {
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public required string Status { get; set; }
    }
}
