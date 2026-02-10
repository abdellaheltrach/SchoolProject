using School.Domain.Entities.Identity;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Context;
using School.Infrastructure.Repositories.Interfaces;

namespace School.Infrastructure.Repositories
{
    public class UserRefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IUserRefreshTokenRepository
    {
        public UserRefreshTokenRepository(AppDbContext context) : base(context)
        {
        }
    }
}
