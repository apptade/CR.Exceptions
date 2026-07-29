using System.Collections.Immutable;

namespace CR.Exceptions.Map;

public sealed record class ExceptionErrorDescriptor
{
    public string Code { get; init; }
    public ImmutableArray<CrError> Errors { get; init; }
    public Func<ImmutableArray<CrError>, CrException>? ExceptionFactory { get; init; }

    public ExceptionErrorDescriptor(string code, ImmutableArray<CrError> errors, Func<ImmutableArray<CrError>, CrException>? exceptionFactory = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        CrErrorValidator.ThrowExceptionIfInvalid(errors);

        Code = code;
        Errors = errors;
        ExceptionFactory = exceptionFactory;
    }
}