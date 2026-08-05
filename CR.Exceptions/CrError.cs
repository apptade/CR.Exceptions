namespace CR.Exceptions;

public record class CrError
{
    public string Code { get; }
    public string Message { get; }

    public CrError(string code, string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);
        ArgumentException.ThrowIfNullOrEmpty(message);

        Code = code;
        Message = message;
    }
}