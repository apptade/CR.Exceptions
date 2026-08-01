namespace CR.Exceptions;

public record class CrError
{
    public string Code { get; init; }
    public string Message { get; init; }

    public CrError(string code, string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);
        ArgumentException.ThrowIfNullOrEmpty(message);

        Code = code;
        Message = message;
    }
}