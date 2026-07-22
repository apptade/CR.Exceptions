namespace CR.Exceptions;

public abstract class ConflictException : CrException
{
    protected ConflictException(IReadOnlyCollection<CrError> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}