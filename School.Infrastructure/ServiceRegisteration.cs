using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Infrastructure.Context;
namespace School.Infrastructure
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity
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
                option.SignIn.RequireConfirmedEmail = false;


            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); ;
            #endregion


            #region Authentication Service

            //read JWT settings from appsettings.json file and bind it to JwtSettings class
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);

            services.AddSingleton(jwtSettings);
            #endregion









            return services;
        }
    }
}
