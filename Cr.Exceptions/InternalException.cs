namespace Cr.Exceptions;

public class InternalException : CrException
{
    public InternalException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "An unexpected internal error occurred.", innerException)
    {
    }

    public InternalException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}