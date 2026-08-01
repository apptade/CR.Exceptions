using CR.Exceptions.Mapping;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.AspNet.Mapping;

public abstract class TypeMap<TValue> : Map<Type, TValue>
{
    protected TypeMap(FrozenDictionary<Type, TValue> dictionary) : base(dictionary) { }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out TValue value)
    {
        ArgumentNullException.ThrowIfNull(exception);

        for (var type = exception.GetType(); type is not null; type = type.BaseType)
        {
            if (TryGetValue(type, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }
}