using CR.Exceptions.Extensions;

namespace CR.Exceptions.Mapping;

public sealed class ErrorMapBuilder
{
    private readonly RegistrationCollection<ErrorRegistration> _collection = new();

    public ErrorMapBuilder Add(ErrorRegistration registration)
    {
        _collection.Add(registration);
        return this;
    }

    public ErrorMapBuilder AddRange(IEnumerable<ErrorRegistration> registrations)
    {
        _collection.AddRange(registrations);
        return this;
    }

    public ErrorMap Build()
    {
        return new(_collection.Items.ToUniqueFrozenDictionary(
            keySelector: k => k.Code,
            elementSelector: v => v,
            comparer: StringComparer.Ordinal));
    }
}