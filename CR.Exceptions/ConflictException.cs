namespace CR.Exceptions;

public abstract class ConflictException : CrException
{
    protected ConflictException(CrError[] errors, string message, Exception? innerException = null) : base(errors, message, innerException)
    {
    }
}