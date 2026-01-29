using Microsoft.AspNetCore.Identity;
using School.Domain.Entities.Identity;
using School.Service.Services.Interfaces;

namespace School.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        #endregion

        #region Constructors
        public AuthorizationService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion

        #region Methods
        public async Task<bool> AddRoleAsync(string roleName)
        {
            var identityRole = new Role();
            identityRole.Name = roleName;
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        #endregion


    }
}
