namespace ServiceBookingPlatform.Models.Dtos.Booking
{
    public class CreateBookingDto
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public required string Status { get; set; }
    }
}
