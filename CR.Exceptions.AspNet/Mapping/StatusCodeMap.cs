using CR.Exceptions.Mapping;
using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public class StatusCodeMap : TypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> dictionary) : base(dictionary) { }
}