namespace ServiceBookingPlatform.Models.Dtos.User
{
    public class UserLogInResponseDto
    {
        public string? Email { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }

    }
}
