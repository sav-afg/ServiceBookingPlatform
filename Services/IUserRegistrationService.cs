using ServiceBookingPlatform.Models;

using FieldValidatorAPI;
using ServiceBookingPlatform.Models.Dtos.User;
namespace ServiceBookingPlatform.Services
{
    public interface IUserRegistrationService
    {
        public Task<bool> EmailExistsAsync(string email);
        public Task<(bool Success, string Message)> RegisterUserAsync(UserDto userDto);

        public ValidationResult ValidateUserDto(UserDto userDto);
    }
}
