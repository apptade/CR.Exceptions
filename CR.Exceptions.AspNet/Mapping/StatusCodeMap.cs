using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.AspNet;

public class StatusCodeMap : TypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out int code)
        => TryGetByHierarchy(exception.GetType(), out code);
}