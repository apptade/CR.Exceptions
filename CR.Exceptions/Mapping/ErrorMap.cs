using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CR.Exceptions.Mapping;

public sealed class ErrorMap : Map<string, ErrorRegistration>
{
    internal ErrorMap(FrozenDictionary<string, ErrorRegistration> dictionary) : base(dictionary) { }

    public ImmutableArray<CrError> Get(string code)
        => GetValue(code).Errors;

    public bool TryGet(string code, out ImmutableArray<CrError> errors)
    {
        if (TryGetValue(code, out var value))
        {
            errors = value.Errors;
            return true;
        }

        errors = [];
        return false;
    }
}