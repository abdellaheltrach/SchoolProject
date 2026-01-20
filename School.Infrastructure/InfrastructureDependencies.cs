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
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IDepartementRepository, DepartementRepository>();
            services.AddTransient<IInstructorRepository, InstructorRepository>();





            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }

    }
}
