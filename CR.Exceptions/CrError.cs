namespace CR.Exceptions;

public sealed record class CrError
{
    public string Code { get; init; }
    public string Message { get; init; }

    public CrError(string code, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Message = message;
    }
}