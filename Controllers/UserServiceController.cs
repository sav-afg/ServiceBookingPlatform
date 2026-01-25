using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBookingPlatform.Models.Dtos.Service;
using ServiceBookingPlatform.Services;

namespace ServiceBookingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserServiceController(IUserServiceService Service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ServiceDto>>> GetAllServices()
        {
            var services = await Service.GetAllServicesAsync();

            if (services.Count == 0)
            {
                return NotFound("No services found.");
            }

            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
        {
            var service = await Service.GetServiceByIdAsync(id);
            return service is null
                ? NotFound($"Service with ID {id} was not found")
                : Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceDto>> AddService(CreateServiceDto service)
        {
            var createdService = await Service.CreateServiceAsync(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.Id }, createdService);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ServiceDto>> UpdateService(int id, UpdateServiceDto service)
        {
            var updatedService = await Service.UpdateServiceAsync(id, service);
            return updatedService is null
                ? NotFound($"Service with ID {id} was not found")
                : Ok(updatedService);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var result = await Service.DeleteServiceAsync(id);
            return result
                ? Ok($"Service with ID {id} successfully deleted.")
                : NotFound($"Service with ID {id} was not found");
        }
    }
}
