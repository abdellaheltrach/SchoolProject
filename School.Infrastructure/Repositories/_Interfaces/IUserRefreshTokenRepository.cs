using School.Domain.Entities.Identity;
using School.Infrastructure.Bases;

namespace School.Infrastructure.Repositories.Interfaces
{
    public interface IUserRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
    }
}
