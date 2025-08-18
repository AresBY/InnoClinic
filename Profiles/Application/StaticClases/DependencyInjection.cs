using InnoClinic.Profiles.Application.Behaviors;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace InnoClinic.Profiles.Application.StaticClases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
