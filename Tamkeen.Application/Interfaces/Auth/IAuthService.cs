using Tamkeen.Application.DTOs.Auth;
namespace Tamkeen.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, ConfirmEmailResponseDto? Data, string Message)> ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<(bool Success, AuthResponseDto? Data, string Message)> LoginAsync(LoginDto dto);
        Task<(bool Success, string Message)> ResendCodeAsync(string email);
    }
}
