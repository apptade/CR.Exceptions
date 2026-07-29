using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CR.Exceptions.Map;

public sealed class ErrorMap
{
    private readonly FrozenDictionary<string, ErrorRegistration> _map;

    public ErrorMap(FrozenDictionary<string, ErrorRegistration> map)
    {
        _map = map;
    }

    public ImmutableArray<CrError> GetOrDefault(string code)
    {
        TryGet(code, out var errors);
        return errors;
    }

    public bool TryGet(string code, out ImmutableArray<CrError> errors)
    {
        if (_map.TryGetValue(code, out var descriptor))
        {
            errors = descriptor.Errors;
            return true;
        }

        errors = default;
        return false;
    }
}