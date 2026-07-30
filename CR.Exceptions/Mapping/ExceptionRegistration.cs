using System.Collections.Immutable;

namespace CR.Exceptions.Mapping;

public sealed record class ExceptionRegistration
{
    public ErrorRegistration Error { get; init; }
    public Func<ImmutableArray<CrError>, CrException> Factory { get; init; }

    public ExceptionRegistration(ErrorRegistration error, Func<ImmutableArray<CrError>, CrException> factory)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(factory);

        Error = error;
        Factory = factory;
    }
}