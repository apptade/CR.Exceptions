namespace Cr.Exceptions;

public class ConflictException : CrException
{
    public ConflictException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The requested operation could not be completed due to a conflict.", innerException)
    {
    }

    public ConflictException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}