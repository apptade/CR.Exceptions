namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactoryBuilder : MapBuilder<string, ExceptionRegistration>
{
    public ExceptionFactoryBuilder Add(ExceptionRegistration registration)
    {
        Add(registration.Definition.Code, registration);
        return this;
    }

    public ExceptionFactoryBuilder AddRange(IEnumerable<ExceptionRegistration> registrations)
    {
        foreach (var registration in registrations) Add(registration);
        return this;
    }

    public ExceptionFactory Build()
    {
        return new(BuildFrozenDictionary(comparer: StringComparer.Ordinal));
    }
}