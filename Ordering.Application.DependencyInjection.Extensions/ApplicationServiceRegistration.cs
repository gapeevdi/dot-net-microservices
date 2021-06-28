using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviors;
using Ordering.Application.Mappings;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(MappingProfile).Assembly;

            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddMediatR(applicationAssembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
