using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet;

public sealed partial class CrExceptionHandler
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Cannot write exception response because the response has already started.")]
    private static partial void LogResponseAlreadyStarted(ILogger logger, Exception exception);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "No HTTP status code mapping found for exception type '{ExceptionType}'. Using 500 Internal Server Error.")]
    private static partial void LogMissingHttpStatusMapping(ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Application exception of type '{ExceptionType}' occurred.")]
    private static partial void LogApplicationException(ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "An unexpected exception of type '{ExceptionType}' occurred.")]
    private static partial void LogUnhandledException(ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "ProblemDetails extension key '{Key}' already exists. The value was overwritten.")]
    private static partial void LogProblemDetailsExtensionOverwritten(ILogger logger, string key);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to write ProblemDetails response.")]
    private static partial void LogFailedToWriteProblemDetails(ILogger logger, Exception exception);
}