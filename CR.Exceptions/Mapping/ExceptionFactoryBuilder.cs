using System.Collections.Frozen;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactoryBuilder
{
    private readonly MappingSource<string, ExceptionRegistration> _source = new();

    public ExceptionFactoryBuilder Add(ExceptionRegistration registration)
    {
        _source.Add(registration.Definition.Code, registration);
        return this;
    }

    public ExceptionFactoryBuilder AddRange(IEnumerable<ExceptionRegistration> registrations)
    {
        foreach (var registration in registrations) Add(registration);
        return this;
    }

    public ExceptionFactory Build()
    {
        return new(_source.Map.ToFrozenDictionary(comparer: StringComparer.Ordinal));
    }
}