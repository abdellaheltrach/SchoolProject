using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.DependencyInjection;
using School.Domain.Entities.Identity;
using School.Infrastructure.Context;
namespace School.Infrastructure
{
    public static class ServiceIdentityRegisteration
    {
        public static IServiceCollection AddIdentityServiceRegisteration(this IServiceCollection services)
        {
            //needed 		<FrameworkReference Include="Microsoft.AspNetCore.App" />

            services.AddIdentity<User, IdentityRole<int>>(option =>
            {
                // Password settings.
                //checked already by FluentValidation.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 8;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = false;//added in FluentValidation
                option.SignIn.RequireConfirmedEmail = true;


            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); ;
            return services;
        }
    }
}
