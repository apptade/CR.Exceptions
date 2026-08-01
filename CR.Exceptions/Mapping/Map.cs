using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public abstract class Map<TKey, TValue> where TKey : notnull
{
    private readonly FrozenDictionary<TKey, TValue> _dictionary;
    public IReadOnlyDictionary<TKey, TValue> Dictionary => _dictionary;

    protected Map(FrozenDictionary<TKey, TValue> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        _dictionary = dictionary;
    }

    protected TValue GetValue(TKey key)
        => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException($"Key '{key}' in map is not found.");

    protected bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _dictionary.TryGetValue(key, out value);
}