using System.Collections.Frozen;

namespace CR.Exceptions.Mapping;

public sealed class ErrorMapBuilder
{
    private readonly MappingSource<string, ErrorRegistration> _source = new();

    public ErrorMapBuilder Add(ErrorRegistration registration)
    {
        _source.Add(registration.Code, registration);
        return this;
    }

    public ErrorMapBuilder AddRange(IEnumerable<ErrorRegistration> registrations)
    {
        foreach (var registration in registrations) Add(registration);
        return this;
    }

    public ErrorMap Build()
    {
        return new(_source.Map.ToFrozenDictionary(comparer: StringComparer.Ordinal));
    }
}