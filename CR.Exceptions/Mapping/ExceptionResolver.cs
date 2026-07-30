using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionResolver
{
    private readonly FrozenDictionary<string, ExceptionRegistration> _map;

    public ExceptionResolver(FrozenDictionary<string, ExceptionRegistration> map)
    {
        _map = map;
    }

    public CrException Resolve(string code)
    {
        if (TryResolve(code, out var exception))
        {
            return exception;
        }

        throw new KeyNotFoundException($"Exception with code '{code}' is not found.");
    }

    public bool TryResolve(string code, [MaybeNullWhen(false)] out CrException exception)
    {
        if (_map.TryGetValue(code, out var registration))
        {
            exception = registration.Factory(registration.Error.Errors);
            return true;
        }

        exception = default;
        return false;
    }
}