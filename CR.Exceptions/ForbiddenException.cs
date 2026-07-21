namespace CR.Exceptions;

public abstract class ForbiddenException : CrException
{
    protected ForbiddenException(IReadOnlyCollection<Error> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}