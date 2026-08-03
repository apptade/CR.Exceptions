using CR.Exceptions.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.AspNet.Mapping;

public class LogLevelMap : TypeMap<LogLevel>
{
    internal LogLevelMap(FrozenDictionary<Type, LogLevel> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out LogLevel level)
        => TrySearchValue(exception.GetType(), out level);
}