using System.Collections.Immutable;

namespace CR.Exceptions.Mapping;

public record class ExceptionRegistration
{
    public ErrorRegistration Definition { get; init; }
    public Func<ImmutableArray<CrError>, CrException> Factory { get; init; }

    public ExceptionRegistration(ErrorRegistration definition, Func<ImmutableArray<CrError>, CrException> factory)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(factory);

        Definition = definition;
        Factory = factory;
    }
}