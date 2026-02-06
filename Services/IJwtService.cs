using ServiceBookingPlatform.Models.Dtos.User;

namespace ServiceBookingPlatform.Services
{
    public interface IJwtService
    {
        public Task<UserLogInResponseDto?> Authenticate(UserLogInRequestDto request);
    }
}
