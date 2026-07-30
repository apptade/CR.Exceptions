using CR.Exceptions.Extensions;

namespace CR.Exceptions.Mapping;

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
        return new(_registrations.ToUniqueFrozenDictionary(
            keySelector: k => k.Code,
            elementSelector: v => v,
            comparer: StringComparer.Ordinal));
    }
}