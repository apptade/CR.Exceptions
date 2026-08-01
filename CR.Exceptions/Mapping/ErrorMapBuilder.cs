namespace CR.Exceptions.Mapping;

public sealed class ErrorMapBuilder : MapBuilder<string, ErrorRegistration>
{
    public ErrorMapBuilder Add(ErrorRegistration registration)
    {
        Add(registration.Code, registration);
        return this;
    }

    public ErrorMapBuilder AddRange(IEnumerable<ErrorRegistration> registrations)
    {
        foreach (var registration in registrations) Add(registration);
        return this;
    }

    public ErrorMap Build()
    {
        return new(BuildFrozenDictionary(comparer: StringComparer.Ordinal));
    }
}