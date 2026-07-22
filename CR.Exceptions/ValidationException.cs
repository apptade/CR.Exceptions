namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(CrError[] errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}