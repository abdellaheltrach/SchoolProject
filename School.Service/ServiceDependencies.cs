using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;
using School.Service.Services;
using School.Service.Services.Interfaces;

namespace School.Service
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            return services;
        }
    }
}
