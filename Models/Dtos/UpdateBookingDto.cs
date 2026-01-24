namespace ServiceBookingPlatform.Models.Dtos
{
    public class UpdateBookingDto
    {
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public required string Status { get; set; }
    }
}
