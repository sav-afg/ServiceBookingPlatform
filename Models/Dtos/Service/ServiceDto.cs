using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.Service
{
    public class ServiceDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public required string ServiceName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string ServiceType { get; set; }

        [StringLength(500)]
        public string ServiceDescription { get; set; } = string.Empty;
    }
}
