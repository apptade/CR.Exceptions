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
            services.AddCustomProblemDetails();
            services.AddExceptionHandler<CrExceptionHandler>();

            return services;
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