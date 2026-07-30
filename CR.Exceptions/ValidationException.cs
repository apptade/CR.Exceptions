using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "One or more validation errors occurred.", innerException)
    {
    }

    protected ValidationException(ImmutableArray<CrError> errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}