using CR.Exceptions.Mapping;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.AspNet.Mapping;

public class StatusCodeMap : TypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out int code)
        => TrySearchValue(exception.GetType(), out code);
}