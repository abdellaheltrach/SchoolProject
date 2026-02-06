using School.Domain.Entities.Identity;
using School.Domain.Results;
using System.Security.Claims;

namespace School.Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResult> GenerateJwtTokenAsync(User user);
        public (bool IsValid, List<Claim>?, string ErrorMessage) ValidateJwtToken(string AccessToken);
        public Task<JwtAuthResult> RefreshJwtTokenAsync(string accessToken, string refreshToken);
        public Task<bool> ConfirmEmail(int? userId, string? code);
        public Task<string> SendResetPasswordCode(string Email);
        public Task<string> ConfirmResetPassword(string Code, string Email);
        public Task<string> ResetPassword(string Email, string Password);

    }
}
