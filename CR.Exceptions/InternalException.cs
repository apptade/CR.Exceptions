using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class InternalException : CrException
{
    protected InternalException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "An unexpected internal error occurred.", innerException)
    {
    }

    protected InternalException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}