namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}