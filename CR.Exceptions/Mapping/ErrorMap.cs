using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CR.Exceptions.Mapping;

public sealed class ErrorMap
{
    private readonly FrozenDictionary<string, ErrorRegistration> _map;

    internal ErrorMap(FrozenDictionary<string, ErrorRegistration> map)
    {
        _map = map;
    }

    public ImmutableArray<CrError> Get(string code)
    {
        if (TryGet(code, out var errors))
        {
            return errors;
        }

        throw new KeyNotFoundException($"Collection with code '{code}' is not found.");
    }

    public bool TryGet(string code, out ImmutableArray<CrError> errors)
    {
        if (_map.TryGetValue(code, out var descriptor))
        {
            errors = descriptor.Errors;
            return true;
        }

        errors = [];
        return false;
    }
}