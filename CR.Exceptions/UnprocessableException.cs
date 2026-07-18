namespace CR.Exceptions;

public abstract class UnprocessableException : CrException
{
    protected UnprocessableException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}