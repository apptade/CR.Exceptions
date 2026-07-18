namespace CR.Exceptions;

public abstract class ForbiddenException : CrException
{
    protected ForbiddenException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}