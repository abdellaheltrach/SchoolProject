using School.Domain.Entities.Identity;
using School.Domain.Responses;

namespace School.Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResponse> GetJWTToken(User user);

    }
}
