namespace CR.Exceptions;

public abstract class CrException : Exception
{
    public string Code { get; }

    protected CrException(string code, string message, Exception? innerException = null) : base(message, innerException)
    {
        Code = code;
    }
}