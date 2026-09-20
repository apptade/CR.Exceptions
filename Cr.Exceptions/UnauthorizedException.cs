namespace Cr.Exceptions;

public class UnauthorizedException : CrException
{
    public UnauthorizedException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "Authentication is required to access this resource.", innerException)
    {
    }

    public UnauthorizedException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}