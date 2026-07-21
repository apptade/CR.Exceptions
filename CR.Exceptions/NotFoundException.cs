namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(IReadOnlyCollection<Error> errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}