namespace ServiceBookingPlatform.Models.Dtos
{
    public class ServiceDto
    {
        public required string ServiceName { get; set; }
        public required string ServiceType { get; set; }
        public string ServiceDescription { get; set; } = string.Empty;
    }
}
