namespace CR.Exceptions;

public abstract class UnprocessableException : CrException
{
    protected UnprocessableException(IReadOnlyCollection<CrError> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}