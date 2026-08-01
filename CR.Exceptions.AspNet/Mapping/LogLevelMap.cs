using Microsoft.Extensions.Logging;
using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class LogLevelMap : TypeMap<LogLevel>
{
    internal LogLevelMap(FrozenDictionary<Type, LogLevel> dictionary) : base(dictionary)
    {
    }
}