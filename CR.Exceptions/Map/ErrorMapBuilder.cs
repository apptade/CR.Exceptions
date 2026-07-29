using System.Collections.Frozen;

namespace CR.Exceptions.Map;

public sealed class ErrorMapBuilder
{
    private readonly List<ErrorRegistration> _registrations = [];

    public ErrorMapBuilder Add(ErrorRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);

        _registrations.Add(registration);

        return this;
    }

    public ErrorMapBuilder AddRange(IEnumerable<ErrorRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(registrations);

        foreach (var registration in registrations) Add(registration);

        return this;
    }

    public ErrorMap Build()
    {
        return new(_registrations.ToFrozenDictionary(keySelector: k => k.Code, comparer: StringComparer.Ordinal));
    }
}