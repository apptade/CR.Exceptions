namespace CR.Exceptions;

public abstract class ConflictException : CrException
{
    protected ConflictException(CrError[] errors, Exception? innerException = null)
        : base(errors, "The requested operation could not be completed due to a conflict.", innerException)
    {
    }

    protected ConflictException(CrError[] errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}