using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.AspNet.Mapping;

public abstract class ExceptionTypeMap<TValue>
{
    private readonly FrozenDictionary<Type, TValue> _map;

    protected ExceptionTypeMap(FrozenDictionary<Type, TValue> map)
    {
        _map = map;
    }

    public bool TryFind(CrException exception, [MaybeNullWhen(false)] out TValue value)
    {
        ArgumentNullException.ThrowIfNull(exception);

        for (var type = exception.GetType(); type is not null; type = type.BaseType)
        {
            if (_map.TryGetValue(type, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }
}