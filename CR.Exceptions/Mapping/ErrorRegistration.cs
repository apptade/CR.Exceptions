using CR.Exceptions.Extensions;
using System.Collections.Immutable;

namespace CR.Exceptions.Mapping;

public sealed record class ErrorRegistration
{
    public string Code { get; init; }
    public ImmutableArray<CrError> Errors { get; init; }

    public ErrorRegistration(string code, ImmutableArray<CrError> errors)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        errors.ThrowIfEmptyOrContainsNull();

        Code = code;
        Errors = errors;
    }
}