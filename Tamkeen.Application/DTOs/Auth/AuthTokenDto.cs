namespace Tamkeen.Application.DTOs.Auth
{
    public class AuthTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
