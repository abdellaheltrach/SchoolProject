using School.Domain.Entities.Identity;
using School.Domain.Results;

namespace School.Service.Services.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<bool> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExistByName(string roleName);
        Task<bool> EditRoleAsync(int RoleId, string newRoleName);
        Task<bool> IsRoleExistById(int roleId);
        Task<string> DeleteRoleAsync(int id);
        Task<List<Role>> GetRolesList();
        Task<Role?> GetRoleById(int id);
        Task<ManageUserRolesResult> ManageUserRolesData(User user);
        Task<string> UpdateUserRoles(int UserId, List<UserRoles> userRoles);
        public Task<ManageUserClaimsResult> ManageUserClaimData(User user);

    }
}
