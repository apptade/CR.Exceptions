using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class CrException : Exception
{
    public ImmutableArray<CrError> Errors { get; }

    protected CrException(ImmutableArray<CrError> errors, string? message = null, Exception? innerException = null) : base(message, innerException)
    {
        errors.ThrowIfEmptyOrContainsNull();

        Errors = errors;
    }
}