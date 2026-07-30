using CR.Exceptions.Extensions;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactoryBuilder
{
    private readonly List<ExceptionRegistration> _registrations = [];

    public ExceptionFactoryBuilder Add(ExceptionRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);

        _registrations.Add(registration);
        return this;
    }

    public ExceptionFactoryBuilder AddRange(IEnumerable<ExceptionRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(registrations);

        foreach (var registration in registrations) Add(registration);
        return this;
    }

    public ExceptionFactory Build()
    {
        return new(_registrations.ToUniqueFrozenDictionary(
            keySelector: k => k.Definition.Code,
            elementSelector: v => v,
            comparer: StringComparer.Ordinal));
    }
}