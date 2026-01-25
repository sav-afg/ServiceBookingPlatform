using Microsoft.EntityFrameworkCore;
using ServiceBookingPlatform.Data;
using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos.Booking;
using ServiceBookingPlatform.Models.Dtos.Service;

namespace ServiceBookingPlatform.Services
{
    public class UserServiceService(AppDbContext Db) : IUserServiceService
    {

        private IQueryable<ServiceDto> GetServiceQuery()
        {
            return Db.Services
                .Select(b => new ServiceDto
                {
                    Id = b.Id,
                    ServiceName = b.ServiceName,
                    ServiceType = b.ServiceType,
                    ServiceDescription = b.ServiceDescription
                });
        }

        public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto newService)
        {
            var service = new Service
            {
                ServiceName = newService.ServiceName,
                ServiceType = newService.ServiceType,
                ServiceDescription = newService.ServiceDescription
            };

            Db.Services.Add(service);
            await Db.SaveChangesAsync();

            return (await GetServiceByIdAsync(service.Id))!;
        }

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            var service = Db.Services.FirstOrDefault(b => b.Id == serviceId);

            if (service == null)
                return false;

            Db.Services.Remove(service);
            await Db.SaveChangesAsync();
            return true;
        }

        public async Task<List<ServiceDto>> GetAllServicesAsync()
        {
            return await GetServiceQuery().ToListAsync();
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(int serviceId)
        {
            return await GetServiceQuery()
                .FirstOrDefaultAsync(b => b.Id == serviceId);
        }

        public async Task<bool> ServiceExistsAsync(int serviceId)
        {
            return await Db.Services.AnyAsync(s => s.Id == serviceId);
        }

        public async Task<ServiceDto?> UpdateServiceAsync(int serviceId, UpdateServiceDto updatedService)
        {
            var existingService = await Db.Services.FindAsync(serviceId);

            if (existingService == null)
                return null;

            existingService.ServiceDescription = updatedService.ServiceDescription;
            await Db.SaveChangesAsync();

            return await GetServiceByIdAsync(serviceId);
        }
    }
}
