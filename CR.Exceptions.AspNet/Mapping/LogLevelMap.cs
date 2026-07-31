using Microsoft.Extensions.Logging;
using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class LogLevelMap : ExceptionTypeMap<LogLevel>
{
    internal LogLevelMap(FrozenDictionary<Type, LogLevel> map) : base(map)
    {
    }
}