using School.Core.Features.Users.Commands.Models;
using School.Domain.Entities.Identity;


namespace School.Core.Mapping.UsersMapping
{
    public partial class UserProfile
    {
        public void AddUserCommandMapping()
        {
            CreateMap<AddUserCommand, User>();
        }






    }
}

