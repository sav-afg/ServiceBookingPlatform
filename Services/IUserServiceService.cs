using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Models.Dtos.Service;

namespace ServiceBookingPlatform.Services
{
    public interface IUserServiceService
    {
        public Task<bool> ServiceExistsAsync(int serviceId);

        Task<List<ServiceDto>> GetAllServicesAsync();

        Task<ServiceDto?> GetServiceByIdAsync(int serviceId);

        Task<ServiceDto> CreateServiceAsync(CreateServiceDto newService);

        Task<ServiceDto?> UpdateServiceAsync(int serviceId, UpdateServiceDto updatedService);

        Task<bool> DeleteServiceAsync(int serviceId);

    }
}
