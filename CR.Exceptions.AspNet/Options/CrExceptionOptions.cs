namespace CR.Exceptions.AspNet.Options;

public sealed class CrExceptionOptions
{
    public ExceptionMappingOptions ExceptionMapping { get; init; } = new();
    public ProblemDetailsOptions ProblemDetails { get; init; } = new();
}