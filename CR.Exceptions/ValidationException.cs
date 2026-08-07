using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The provided data is invalid.", innerException)
    {
    }

    protected ValidationException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}