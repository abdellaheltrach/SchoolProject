using School.Domain.Entities.Identity;

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
    }
}
