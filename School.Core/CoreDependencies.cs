using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using School.Core.Behaviors;
using System.Reflection;
namespace School.Core
{
    public static class CoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            //Configuration Of Mediator
            services.AddMediatR(Configuration => Configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            //registering of the auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            // registering Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // registering ValidationBehavior in dependency injection container
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;

        }

    }
}
