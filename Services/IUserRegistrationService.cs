using ServiceBookingPlatform.Models;
using ServiceBookingPlatform.Models.Dtos;

using FieldValidatorAPI;
namespace ServiceBookingPlatform.Services
{
    public interface IUserRegistrationService
    {
        public Task<bool> EmailExistsAsync(string email);
        public Task<(bool Success, string Message)> RegisterUserAsync(UserDto userDto);

        public ValidationResult ValidateUserDto(UserDto userDto);
    }
}
