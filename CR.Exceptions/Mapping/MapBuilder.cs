using System.Collections.Frozen;

namespace CR.Exceptions.Mapping;

public abstract class MapBuilder<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _map;

    protected MapBuilder()
    {
        _map = [];
    }

    protected MapBuilder(int startCapacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(startCapacity);
        _map = new(capacity: startCapacity);
    }

    protected void AddPair(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        if (!_map.TryAdd(key, value))
        {
            throw new ArgumentException(
                $"The key '{key}' of type '{key.GetType().FullName}' has already been added.", nameof(key));
        }
    }

    protected FrozenDictionary<TKey, TValue> BuildFrozenDictionary(IEqualityComparer<TKey>? comparer = null)
        => _map.ToFrozenDictionary(comparer);
}