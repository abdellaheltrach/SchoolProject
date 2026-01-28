using School.Domain.Entities.Identity;
using School.Domain.Results;

namespace School.Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResult> GenerateJwtTokenAsync(User user);

    }
}
