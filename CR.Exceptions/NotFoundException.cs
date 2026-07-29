using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The requested resource was not found.", innerException)
    {
    }

    protected NotFoundException(ImmutableArray<CrError> errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}