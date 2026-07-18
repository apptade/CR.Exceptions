namespace CR.Exceptions;

public abstract class NotFoundException : CrException
{
    protected NotFoundException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}