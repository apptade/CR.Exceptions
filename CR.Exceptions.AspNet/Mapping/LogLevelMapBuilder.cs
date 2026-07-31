using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class LogLevelMapBuilder : ExceptionTypeMapBuilder<LogLevel>
{
    public LogLevelMap Build()
    {
        return new(BuildMap());
    }
}