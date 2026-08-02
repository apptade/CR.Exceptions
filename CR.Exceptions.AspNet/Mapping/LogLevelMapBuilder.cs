using CR.Exceptions.Mapping;
using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Mapping;

public class LogLevelMapBuilder : TypeMapBuilder<LogLevel>
{
    public LogLevelMap Build()
    {
        return new(BuildFrozenDictionary());
    }

    protected override void ThrowIfInvalidValue(LogLevel value)
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), value, $"The value '{value}' is not a valid {nameof(LogLevel)}.");
        }

        if (value == LogLevel.None)
        {
            throw new ArgumentException(
                $"{nameof(LogLevel)}.{nameof(LogLevel.None)} cannot be used for exception mapping.", nameof(value));
        }
    }
}