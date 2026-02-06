using School.Domain.Entities.Identity;

namespace School.Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string> AddUserAsync(User user, string password);

    }
}
