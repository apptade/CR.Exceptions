using System.Collections.Frozen;

namespace CR.Exceptions.Map;

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
        return new(_registrations.ToFrozenDictionary(keySelector: k => k.Error.Code, comparer: StringComparer.Ordinal));
    }
}