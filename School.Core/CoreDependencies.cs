using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace School.Core
{
    public static class CoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            //Configuration Of Mediator
            services.AddMediatR(Configuration => Configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            //Congigurations of the auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;

        }

    }
}
