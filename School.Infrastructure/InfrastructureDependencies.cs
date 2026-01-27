using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Bases;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;
using School.Infrastructure.Repositories.Interfaces;

namespace School.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {

            // Specific Repositories
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IDepartmentRepository, DepartementRepository>();
            services.AddTransient<IInstructorRepository, InstructorRepository>();
            services.AddTransient<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

            // Generic Repository
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            return services;
        }

    }
}
