using System.Collections.Frozen;

namespace CR.Exceptions.AspNet.Mapping;

public abstract class ExceptionTypeMapBuilder<TValue>
{
    private readonly Dictionary<Type, TValue> _map = [];

    public ExceptionTypeMapBuilder<TValue> Map<TException>(TValue value) where TException : CrException
    {
        ThrowIfInvalidValue(value);

        if (!_map.TryAdd(typeof(TException), value))
        {
            throw new InvalidOperationException(
                $"The exception '{typeof(TException).FullName}' has already been mapped.");
        }

        return this;
    }

    protected FrozenDictionary<Type, TValue> BuildMap() => _map.ToFrozenDictionary();

    protected abstract void ThrowIfInvalidValue(TValue value);
}