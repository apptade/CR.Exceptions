namespace CR.Exceptions.Mapping;

internal sealed class RegistrationCollection<T> where T : class
{
    private readonly List<T> _items = [];
    public IReadOnlyList<T> Items => _items;

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }

    public void AddRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            Add(item);
        }
    }
}