using Microsoft.Extensions.DependencyInjection;
using School.Domain.Entities.Views;
using School.Infrastructure.Bases.GenericRepository;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;
using School.Infrastructure.Repositories.Functions;
using School.Infrastructure.Repositories.Interfaces;
using School.Infrastructure.Repositories.Interfaces.Functions;
using School.Infrastructure.Repositories.Interfaces.Procedures;
using School.Infrastructure.Repositories.Interfaces.Views;
using School.Infrastructure.Repositories.Procedures;
using School.Infrastructure.Repositories.Views;

namespace School.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {

            // Specific Repositories
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IInstructorRepository, InstructorRepository>();
            services.AddTransient<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

            // Generic Repository
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            //views
            services.AddTransient<IViewRepository<DepartementTotalStudentView>, ViewDepartmentRepository>();

            //procedures
            services.AddTransient<IDepartmentStudentCountProcedureRepository, DepartmentStudentCountProcedureRepository>();

            //Functions
            services.AddTransient<IInstructorFunctionsRepository, InstructorFunctionsRepository>();

            return services;
        }

    }
}
