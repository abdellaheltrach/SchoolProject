using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities.Identity;

namespace School.Infrastructure.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultuser = new User()
                {
                    UserName = "admin",
                    Email = "admin@project.com",
                    FullName = "schooladmin",
                    Country = "Morocco",
                    PhoneNumber = "123456",
                    Address = "Casablanca",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManager.CreateAsync(defaultuser, "M1234578_m");
                await _userManager.AddToRoleAsync(defaultuser, "Admin");
            }
        }
    }
}
