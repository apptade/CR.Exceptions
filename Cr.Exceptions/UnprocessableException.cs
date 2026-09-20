namespace Cr.Exceptions;

public class UnprocessableException : CrException
{
    public UnprocessableException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "The request could not be processed.", innerException)
    {
    }

    public UnprocessableException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}