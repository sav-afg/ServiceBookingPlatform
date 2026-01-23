namespace ServiceBookingPlatform.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public BookingStatus Status { get; set; }

        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Completed,
            Cancelled
        }

    }

}
