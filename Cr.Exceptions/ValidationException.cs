namespace Cr.Exceptions;

public class ValidationException : CrException
{
    public ValidationException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The provided data is invalid.", innerException)
    {
    }

    public ValidationException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}