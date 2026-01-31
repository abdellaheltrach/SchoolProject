using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities.Identity;
using School.Domain.Helpers;

namespace School.Infrastructure.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> _roleManager)
        {
            var rolesCount = await _roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {

                await _roleManager.CreateAsync(new Role()
                {
                    Name = AppRolesConstants.Admin
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = AppRolesConstants.User
                });
            }
        }
    }
}
