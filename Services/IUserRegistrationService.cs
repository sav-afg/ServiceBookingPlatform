using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos;

namespace ServiceBookingPlatform.Services
{
    public interface IUserRegistrationService
    {
        public Task<bool> RegisterUserAsync(UserDto userDto);

    }
}
