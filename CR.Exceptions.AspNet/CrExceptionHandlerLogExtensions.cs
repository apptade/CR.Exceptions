using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet;

public static partial class CrExceptionHandlerLogExtensions
{
    private static class LogIds
    {
        private const int BaseId = 33_000;

        public const int ResponseAlreadyStarted = BaseId + 1;
        public const int MissingHttpStatusMapping = BaseId + 2;
        public const int UnknownException = BaseId + 3;
        public const int ProblemDetailsExtensionOverwritten = BaseId + 4;
        public const int FailedToWriteProblemDetails = BaseId + 5;
        public const int CrExceptionOccurred = BaseId + 6;
        public const int MissingLogLevelMapping = BaseId + 7;
    }

    [LoggerMessage(
        EventId = LogIds.ResponseAlreadyStarted,
        Level = LogLevel.Error,
        Message = "Cannot write exception response because the response has already started.")]
    public static partial void LogResponseAlreadyStarted(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = LogIds.MissingHttpStatusMapping,
        Level = LogLevel.Warning,
        Message = "No HTTP status code mapping found for exception type '{ExceptionType}'. Using 500 Internal Server Error.")]
    public static partial void LogMissingHttpStatusMapping(this ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        EventId = LogIds.UnknownException,
        Level = LogLevel.Error,
        Message = "Unknown exception of type '{ExceptionType}' occurred. Default internal errors will be used.")]
    public static partial void LogUnknownException(this ILogger logger, Exception exception, string? exceptionType);

    [LoggerMessage(
        EventId = LogIds.ProblemDetailsExtensionOverwritten,
        Level = LogLevel.Warning,
        Message = "The ProblemDetails extension '{Key}' was overwritten while building the error response.")]
    public static partial void LogProblemDetailsExtensionOverwritten(this ILogger logger, string key);

    [LoggerMessage(
        EventId = LogIds.FailedToWriteProblemDetails,
        Level = LogLevel.Error,
        Message = "Failed to write ProblemDetails response.")]
    public static partial void LogFailedToWriteProblemDetails(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = LogIds.CrExceptionOccurred,
        Message = "CR exception of type '{ExceptionType}' occurred.")]
    public static partial void LogCrExceptionOccurred(this ILogger logger, LogLevel level, Exception exception, string? exceptionType);

    [LoggerMessage(
        EventId = LogIds.MissingLogLevelMapping,
        Level = LogLevel.Information,
        Message = "No log level mapping found for exception type '{ExceptionType}'. Using fallback log level '{FallbackLogLevel}'.")]
    public static partial void LogMissingLogLevelMapping(this ILogger logger, string? exceptionType, LogLevel fallbackLogLevel);
}