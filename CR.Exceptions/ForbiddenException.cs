using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class ForbiddenException : CrException
{
    protected ForbiddenException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "You do not have permission to perform this operation.", innerException)
    {
    }

    protected ForbiddenException(ImmutableArray<CrError> errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}