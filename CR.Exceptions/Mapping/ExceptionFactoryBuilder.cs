using CR.Exceptions.Extensions;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactoryBuilder
{
    private readonly RegistrationCollection<ExceptionRegistration> _collection = new();

    public ExceptionFactoryBuilder Add(ExceptionRegistration registration)
    {
        _collection.Add(registration);
        return this;
    }

    public ExceptionFactoryBuilder AddRange(IEnumerable<ExceptionRegistration> registrations)
    {
        _collection.AddRange(registrations);
        return this;
    }

    public ExceptionFactory Build()
    {
        return new(_collection.Items.ToUniqueFrozenDictionary(
            keySelector: k => k.Definition.Code,
            elementSelector: v => v,
            comparer: StringComparer.Ordinal));
    }
}