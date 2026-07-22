namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(IReadOnlyCollection<CrError> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}