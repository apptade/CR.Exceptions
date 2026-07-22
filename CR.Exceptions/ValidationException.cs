namespace CR.Exceptions;

public abstract class ValidationException : CrException
{
    protected ValidationException(CrError[] errors, Exception? innerException = null)
        : base(errors, "One or more validation errors occurred.", innerException)
    {
    }

    protected ValidationException(CrError[] errors, string message, Exception? innerException = null)
        : base(errors, message, innerException)
    {
    }
}