namespace CR.Exceptions.Mapping;

public sealed class MappingSource<TKey, TValue> where TValue : class where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _map = [];

    public IReadOnlyDictionary<TKey, TValue> Map => _map;

    public void Add(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        if (!_map.TryAdd(key, value))
        {
            throw new ArgumentException(
                $"The key '{key}' of type '{key.GetType().FullName}' has already been added.", nameof(key));
        }
    }
}