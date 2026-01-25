namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserLogInRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
