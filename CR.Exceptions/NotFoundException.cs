namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(IReadOnlyCollection<CrError> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}