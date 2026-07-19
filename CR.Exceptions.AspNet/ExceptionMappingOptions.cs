namespace CR.Exceptions.AspNet;

public sealed class ExceptionMappingOptions
{
    private readonly Dictionary<Type, int> _map = [];

    public ExceptionMappingOptions Map<TException>(int httpStatusCode) where TException : CrException
    {
        if (!_map.TryAdd(typeof(TException), httpStatusCode))
        {
            throw new InvalidOperationException(
                $"The exception '{typeof(TException).FullName}' has already been mapped.");
        }

        return this;
    }

    public int? FindHttpStatusCode(CrException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        for (var type = exception.GetType(); type is not null; type = type.BaseType)
        {
            if (_map.TryGetValue(type, out var httpStatusCode))
            {
                return httpStatusCode;
            }
        }

        return null;
    }
}