using Microsoft.Extensions.DependencyInjection;
using School.Service.AuthServices;
using School.Service.AuthServices.Interfaces;
using School.Service.Services;
using School.Service.Services.Interfaces;

namespace School.Service
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IEmailsService, EmailsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IInstructorService, InstructorService>();

            return services;
        }
    }
}
