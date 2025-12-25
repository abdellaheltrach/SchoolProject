using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;

namespace School.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();
            return services;
        }

    }
}
