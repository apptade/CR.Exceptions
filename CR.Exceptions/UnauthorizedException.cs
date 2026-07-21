namespace CR.Exceptions;

public abstract class UnauthorizedException : CrException
{
    protected UnauthorizedException(IReadOnlyCollection<Error> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}