using School.Domain.Entities.Identity;

namespace School.Service.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<string> GetJWTToken(User user);

    }
}
