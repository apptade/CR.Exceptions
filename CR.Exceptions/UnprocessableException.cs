namespace CR.Exceptions;

public abstract class UnprocessableException : CrException
{
    protected UnprocessableException(CrError[] errors, Exception? innerException = null)
        : base(errors, "The request could not be processed.", innerException)
    {
    }

    protected UnprocessableException(CrError[] errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}