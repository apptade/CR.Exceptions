using System.Diagnostics.CodeAnalysis;

namespace Cr.Exceptions.AspNetCore;

public class StatusCodeMap : TypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out int code)
        => TryGetByHierarchy(exception.GetType(), out code);
}