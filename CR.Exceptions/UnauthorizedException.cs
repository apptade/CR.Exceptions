using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class UnauthorizedException : CrException
{
    protected UnauthorizedException(ImmutableArray<CrError> errors, Exception? innerException = null)
        : base(errors, "Authentication is required to access this resource.", innerException)
    {
    }

    protected UnauthorizedException(ImmutableArray<CrError> errors, string? message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}