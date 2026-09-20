namespace Cr.Exceptions;

public class ForbiddenException : CrException
{
    public ForbiddenException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "You do not have permission to perform this operation.", innerException)
    {
    }

    public ForbiddenException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}