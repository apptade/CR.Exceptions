using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactory
{
    private readonly FrozenDictionary<string, ExceptionRegistration> _map;

    internal ExceptionFactory(FrozenDictionary<string, ExceptionRegistration> map)
    {
        _map = map;
    }

    public CrException Create(string code)
    {
        if (TryCreate(code, out var exception))
        {
            return exception;
        }

        throw new KeyNotFoundException($"Exception with code '{code}' is not found.");
    }

    public bool TryCreate(string code, [MaybeNullWhen(false)] out CrException exception)
    {
        if (_map.TryGetValue(code, out var registration))
        {
            exception = registration.Factory(registration.Definition.Errors);
            return true;
        }

        exception = default;
        return false;
    }
}