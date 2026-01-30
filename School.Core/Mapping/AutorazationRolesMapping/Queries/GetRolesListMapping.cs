using School.Core.Features.Autorazation.Queries.QueriesResponse;
using School.Domain.Entities.Identity;
namespace School.Core.Mapping.AutorazationRolesMapping
{
    public partial class RoleProfile
    {
        public void GetRolesListMapping()
        {
            CreateMap<Role, GetRolesListResponse>();
        }
    }
}