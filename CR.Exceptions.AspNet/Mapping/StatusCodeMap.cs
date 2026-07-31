using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class StatusCodeMap : ExceptionTypeMap<int>
{
    internal StatusCodeMap(FrozenDictionary<Type, int> map) : base(map)
    {
    }
}