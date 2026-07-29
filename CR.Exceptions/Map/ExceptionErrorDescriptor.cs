using System.Collections.Immutable;

namespace CR.Exceptions.Map;

public sealed record class ExceptionErrorDescriptor(
    string Code,
    ImmutableArray<CrError> Errors,
    Func<ImmutableArray<CrError>, CrException>? ExceptionFactory = null);