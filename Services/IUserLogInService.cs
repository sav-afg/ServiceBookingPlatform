using FieldValidatorAPI;
using ServiceBookingPlatform.Models.Dtos.User;

namespace ServiceBookingPlatform.Services
{
    public interface IUserLogInService
    {
        Task<(bool success, string errors)> ValidateUserCredentialsAsync(UserLogInDto userDto);
        public ValidationResult ValidateUserDto(UserLogInDto user);
    }
}
