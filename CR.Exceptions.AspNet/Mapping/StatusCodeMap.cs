using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class StatusCodeMap : TypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> dictionary) : base(dictionary)
    {
    }
}