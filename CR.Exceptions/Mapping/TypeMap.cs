using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public abstract class TypeMap<TValue> : Map<Type, TValue>
{
    protected TypeMap(FrozenDictionary<Type, TValue> dictionary) : base(dictionary) { }

    protected TValue GetByHierarchy(Type? type)
        => TryGetByHierarchy(type, out var value) ? value : throw CreateKeyNotFoundException(type);

    protected bool TryGetByHierarchy(Type? type, [MaybeNullWhen(false)] out TValue value)
    {
        for (; type is not null; type = type.BaseType)
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