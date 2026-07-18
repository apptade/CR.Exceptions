namespace CR.Exceptions;

public abstract class ConflictException : CrException
{
    protected ConflictException(string code, string message, Exception? innerException = null) : base(code, message, innerException)
    {
    }
}