namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(CrError[] errors, Exception? innerException = null)
        : base(errors, "The requested resource was not found.", innerException)
    {
    }

    protected NotFoundException(CrError[] errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}