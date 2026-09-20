using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Cr.Exceptions.AspNetCore;

public class LogLevelMap : TypeMap<LogLevel>
{
    internal LogLevelMap(FrozenDictionary<Type, LogLevel> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out LogLevel level)
        => TryGetByHierarchy(exception.GetType(), out level);
}