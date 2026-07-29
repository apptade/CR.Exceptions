using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class ConflictException : CrException
{
    protected ConflictException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The requested operation could not be completed due to a conflict.", innerException)
    {
    }

    protected ConflictException(ImmutableArray<CrError> errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}