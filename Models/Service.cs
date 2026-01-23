namespace ServiceBookingPlatform.Models
{
    public class Service
    {
        public int Id { get; set; }
        public required string ServiceName { get; set; }
        public required string ServiceType { get; set; }
        //public decimal Price { get; set; }
        //public TimeSpan Duration { get; set; }
    }
}
