using CR.Exceptions.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public class LogLevelMap : TypeMap<LogLevel>
{
    internal LogLevelMap(FrozenDictionary<Type, LogLevel> dictionary) : base(dictionary) { }
}