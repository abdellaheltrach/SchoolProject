using School.Core.Features.Users.Queries.Response;
using School.Domain.Entities.Identity;

namespace School.Core.Mapping.UsersMapping
{
    public partial class UserProfile
    {
        void GetUserByIdQueryMapping()
        {
            CreateMap<User, GetUserByIdQueryResponse>();

        }

    }
}