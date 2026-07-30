using CR.Exceptions.Extensions;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionResolverBuilder
{
    private readonly List<ExceptionRegistration> _registrations = [];

    public ExceptionResolverBuilder Add(ExceptionRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);

        _registrations.Add(registration);

        return this;
    }

    public ExceptionResolverBuilder AddRange(IEnumerable<ExceptionRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(registrations);

        foreach (var registration in registrations) Add(registration);

        return this;
    }

    public ExceptionResolver Build()
    {
        return new(_registrations.ToUniqueFrozenDictionary(
            keySelector: k => k.Definition.Code,
            elementSelector: v => v,
            comparer: StringComparer.Ordinal));
    }
}