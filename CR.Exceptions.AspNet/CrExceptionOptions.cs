namespace CR.Exceptions.AspNet;

public sealed class CrExceptionOptions
{
    public ExceptionStatusCodeOptions StatusCodes { get; init; } = new();
}