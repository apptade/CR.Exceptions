using CR.Exceptions.Mapping;
using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Mapping;

public class LogLevelMapBuilder : MapBuilder<Type, LogLevel>
{
    public LogLevelMapBuilder Map<TException>(LogLevel level) where TException : CrException
    {
        ThrowIfInvalidLevel(level);
        AddPair(typeof(TException), level);

        return this;
    }

    public LogLevelMap Build()
        => new(BuildFrozenDictionary());

    private static void ThrowIfInvalidLevel(LogLevel level)
    {
        if (!Enum.IsDefined(level))
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, $"The value '{level}' is not a valid {nameof(LogLevel)}.");
        }

        if (level is LogLevel.None)
        {
            throw new ArgumentException(
                $"{nameof(LogLevel)}.{nameof(LogLevel.None)} cannot be used for exception mapping.", nameof(level));
        }
    }
}