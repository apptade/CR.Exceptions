namespace CR.Exceptions;

public abstract class UnauthorizedException : CrException
{
    protected UnauthorizedException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}