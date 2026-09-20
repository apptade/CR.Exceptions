namespace Cr.Exceptions;

public class NotFoundException : CrException
{
    public NotFoundException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The requested resource was not found.", innerException)
    {
    }

    public NotFoundException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}