using CR.Exceptions.AspNet.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace CR.Exceptions.AspNet;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCrExceptionDefaultHandling()
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
        {
            return AddCrStatusCodeMapping(services, configurator => configurator.AddDefaultMappings());
        }

        public IServiceCollection AddCrStatusCodeMapping(Action<StatusCodeMapBuilder> configurator)
        {
            ArgumentNullException.ThrowIfNull(configurator);

            var builder = new StatusCodeMapBuilder();
            configurator(builder);

            return services.AddSingleton(builder.Build());
        }

        public IServiceCollection AddCrLogLevelMapping()
        {
            return AddCrLogLevelMapping(services, configurator => configurator.AddDefaultMappings());
        }

        public IServiceCollection AddCrLogLevelMapping(Action<LogLevelMapBuilder> configurator)
        {
            ArgumentNullException.ThrowIfNull(configurator);

            var builder = new LogLevelMapBuilder();
            configurator(builder);

            return services.AddSingleton(builder.Build());
        }

        private IServiceCollection AddCustomProblemDetails()
        {
            return services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
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