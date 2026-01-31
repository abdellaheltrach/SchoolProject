using School.Domain.Entities.Identity;
using School.Domain.Results;
using School.Domain.Results.Requests;

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
        Task<ManageUserClaimsResult> ManageUserClaimData(User user);
        Task<string> UpdateUserClaims(UpdateUserClaimsRequest request);

    }
}
