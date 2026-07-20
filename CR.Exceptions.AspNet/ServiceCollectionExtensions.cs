using Microsoft.Extensions.DependencyInjection;

namespace CR.Exceptions.AspNet;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCrExceptionHandler()
        {
            return services.AddCrExceptionHandler(options =>
            {
                options.AddDefaultMappings();
            });
        }

        public IServiceCollection AddCrExceptionHandler(Action<ExceptionMappingOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(setupAction);

            services.Configure(setupAction);

            services.AddProblemDetails();
            services.AddExceptionHandler<CrExceptionHandler>();

            return services;
        }
    }
}