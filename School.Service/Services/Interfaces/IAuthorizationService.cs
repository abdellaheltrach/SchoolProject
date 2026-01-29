namespace School.Service.Services.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<bool> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExistByName(string roleName);
        Task<bool> EditRoleAsync(int RoleId, string newRoleName);

    }
}
