using System.ComponentModel.DataAnnotations;

namespace ServiceBookingPlatform.Models.Dtos.Service
{
    public class CreateServiceDto
    {
        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Service name must be between 2 and 100 characters")]
        public required string ServiceName { get; set; }

        [Required(ErrorMessage = "Service type is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Service type must be between 2 and 50 characters")]
        public required string ServiceType { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string ServiceDescription { get; set; } = string.Empty;
    }
}
