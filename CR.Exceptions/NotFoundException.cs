namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(CrError[] errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}