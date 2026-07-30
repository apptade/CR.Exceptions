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
                options.StatusCodes.AddDefaultMappings();
            });
        }

        public IServiceCollection AddCrExceptionHandler(Action<CrExceptionOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(setupAction);

            services.Configure(setupAction);

            services.AddProblemDetails();
            services.AddExceptionHandler<CrExceptionHandler>();

            return services;
        }
    }
}