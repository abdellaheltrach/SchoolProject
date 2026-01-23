using AutoMapper;

namespace School.Core.Mapping.UsersMapping
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            AddUserCommandMapping();
        }
    }
}
