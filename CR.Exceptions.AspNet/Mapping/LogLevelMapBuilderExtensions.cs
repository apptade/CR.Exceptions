using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Mapping;

public static class LogLevelMapBuilderExtensions
{
    extension(LogLevelMapBuilder builder)
    {
        public LogLevelMapBuilder AddDefaultMappings()
        {
            builder
                .Map<InternalException>(LogLevel.Error);

            return builder;
        }
    }
}