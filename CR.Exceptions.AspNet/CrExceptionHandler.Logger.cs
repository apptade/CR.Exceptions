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
        Level = LogLevel.Error,
        Message = "An unexpected exception of type '{ExceptionType}' occurred.")]
    private static partial void LogUnhandledException(ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "The ProblemDetails extension '{Key}' was overwritten while building the error response.")]
    private static partial void LogProblemDetailsExtensionOverwritten(ILogger logger, string key);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to write ProblemDetails response.")]
    private static partial void LogFailedToWriteProblemDetails(ILogger logger, Exception exception);

    private static void LogApplicationException(ILogger logger, LogLevel logLevel, Exception exception, string? exceptionType)
    {
        if (logLevel == LogLevel.None)
            return;

        var targetLevel = Enum.IsDefined(logLevel) ? logLevel : LogLevel.Debug;
        LogApplicationExceptionGenerated(logger, targetLevel, exception, exceptionType);
    }

    [LoggerMessage(
        Message = "Application exception of type '{ExceptionType}' occurred.")]
    private static partial void LogApplicationExceptionGenerated(ILogger logger, LogLevel level, Exception exception, string? exceptionType);
}