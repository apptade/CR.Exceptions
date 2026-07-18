using Microsoft.Extensions.DependencyInjection;

namespace CR.Exceptions.AspNet;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCrExceptionHandler(Action<CrExceptionOptions> setupAction)
        {
            services.Configure(setupAction);
            services.AddExceptionHandler<CrExceptionHandler>();

            return services;
        }
    }
}