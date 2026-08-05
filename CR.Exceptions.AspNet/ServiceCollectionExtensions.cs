using Microsoft.Extensions.DependencyInjection;

namespace CR.Exceptions.AspNet;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCrExceptionsCore()
        {
            return services
                .AddCrExceptionHandler()
                .AddCrStatusCodeMapping()
                .AddCrLogLevelMapping();
        }

        public IServiceCollection AddCrExceptionHandler()
        {
            return services
                .AddCustomProblemDetails()
                .AddExceptionHandler<CrExceptionHandler>();
        }

        public IServiceCollection AddCrStatusCodeMapping()
            => AddCrStatusCodeMapping(services, static builder => builder.AddDefaultMappings());

        public IServiceCollection AddCrStatusCodeMapping(Action<StatusCodeMapBuilder> configurator)
        {
            ArgumentNullException.ThrowIfNull(configurator);

            var builder = new StatusCodeMapBuilder();
            configurator(builder);

            return services.AddSingleton(builder.Build());
        }

        public IServiceCollection AddCrLogLevelMapping()
            => AddCrLogLevelMapping(services, static builder => builder.AddDefaultMappings());

        public IServiceCollection AddCrLogLevelMapping(Action<LogLevelMapBuilder> configurator)
        {
            ArgumentNullException.ThrowIfNull(configurator);

            var builder = new LogLevelMapBuilder();
            configurator(builder);

            return services.AddSingleton(builder.Build());
        }

        private IServiceCollection AddCustomProblemDetails()
        {
            return services.AddProblemDetails(static options =>
            {
                options.CustomizeProblemDetails = static context =>
                {
                    var currentActivity = System.Diagnostics.Activity.Current;

                    context.ProblemDetails.Extensions[ProblemDetailsExtensionNames.TraceId] = currentActivity != null
                        ? currentActivity.TraceId.ToHexString()
                        : context.HttpContext.TraceIdentifier;
                };
            });
        }
    }
}