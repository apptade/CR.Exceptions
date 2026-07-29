using System.Collections.Frozen;

namespace CR.Exceptions.Map;

public sealed class ExceptionFactory
{
    private readonly FrozenDictionary<string, ExceptionRegistration> _map;

    public ExceptionFactory(FrozenDictionary<string, ExceptionRegistration> map)
    {
        _map = map;
    }

    public CrException Create(string code)
    {
        if (_map.TryGetValue(code, out var registration))
        {
            return registration.Factory(registration.Error.Errors);
        }

        throw new KeyNotFoundException($"Exception with code '{code}' is not added.");
    }

    public bool TryCreate(string code, out CrException? exception)
    {
        if (_map.TryGetValue(code, out var registration))
        {
            exception = registration.Factory(registration.Error.Errors);
            return true;
        }

        exception = null;
        return false;
    }
}