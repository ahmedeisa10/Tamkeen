namespace Tamkeen.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string token { get; set; }      
        public string email { get; set; }      
        public string fullName { get; set; }   
        public IList<string> roles { get; set; } 
    }
}
