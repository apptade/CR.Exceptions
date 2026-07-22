namespace CR.Exceptions;

public abstract class ForbiddenException : CrException
{
    protected ForbiddenException(CrError[] errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}