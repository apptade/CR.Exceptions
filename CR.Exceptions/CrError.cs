namespace CR.Exceptions;

public record class CrError
{
    public string Code { get; }
    public string? Message { get; }

    public CrError(string code, string? message = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(code);

        Code = code;
        Message = message;
    }
}