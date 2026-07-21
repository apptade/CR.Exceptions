namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(IReadOnlyCollection<Error> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}