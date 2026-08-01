using CR.Exceptions.Mapping;

namespace CR.Exceptions.AspNet.Mapping;

public abstract class TypeMapBuilder<TValue> : MapBuilder<Type, TValue>
{
    public TypeMapBuilder<TValue> Map<TException>(TValue value) where TException : CrException
    {
        ThrowIfInvalidValue(value);
        Add(typeof(TException), value);

        return this;
    }

    protected abstract void ThrowIfInvalidValue(TValue value);
}