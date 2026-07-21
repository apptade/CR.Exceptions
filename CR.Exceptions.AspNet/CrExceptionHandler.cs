using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CR.Exceptions.AspNet;

public sealed class CrExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ExceptionMappingOptions _options;
    private readonly ILogger<CrExceptionHandler> _logger;

    public CrExceptionHandler(
        IProblemDetailsService problemDetailsService,
        IOptions<ExceptionMappingOptions> options,
        ILogger<CrExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _options = options.Value;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (httpContext.Response.HasStarted)
        {
            _logger.LogError(exception, "The response has already started. Cannot write exception response.");
            return false;
        }

        var httpStatusCode = StatusCodes.Status500InternalServerError;
        IReadOnlyCollection<CrException.Error> errors;
        string detail;

        if (exception is CrException crException)
        {
            detail = crException.Message;
            errors = crException.Errors;

            var statusCode = _options.FindHttpStatusCode(crException);

            if (statusCode is null)
            {
                RecordMissingHttpStatusMapping(crException);
            }
            else
            {
                httpStatusCode = statusCode.Value;
            }

            RecordDomainException(crException);
        }
        else
        {
            detail = "An unexpected error occurred.";
            errors = [new(ErrorCodes.InternalError, "An unexpected internal error occurred.")];

            RecordUnhandledException(exception);
        }

        var traceId = Activity.Current?.TraceId.ToHexString() ?? httpContext.TraceIdentifier;
        var title = ReasonPhrases.GetReasonPhrase(httpStatusCode);
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Type = "about:blank",
                Status = httpStatusCode,
                Title = string.IsNullOrWhiteSpace(title) ? "An error occurred" : title,
                Detail = detail,
                Instance = httpContext.Request.Path
            },
        };

        AddProblemDetailsExtension(problemDetailsContext.ProblemDetails, ProblemDetailsExtensionNames.TraceId, traceId);
        AddProblemDetailsExtension(problemDetailsContext.ProblemDetails, ProblemDetailsExtensionNames.Errors, errors);

        httpContext.Response.StatusCode = httpStatusCode;

        var isWritten = await _problemDetailsService.TryWriteAsync(problemDetailsContext);

        if (!isWritten)
            _logger.LogError(exception, "Failed to write ProblemDetails response.");

        return isWritten;
    }

    private void RecordMissingHttpStatusMapping(CrException exception)
    {
        _logger.LogWarning(
            exception,
            "No HTTP status mapping found for exception type '{ExceptionType}' with {ErrorCount} error(s). Using 500 Internal Server Error.",
            exception.GetType().FullName,
            exception.Errors.Count);
    }

    private void RecordDomainException(CrException exception)
    {
        if (!_logger.IsEnabled(LogLevel.Debug))
            return;

        _logger.LogDebug(
            exception,
            "Domain exception of type '{ExceptionType}' occurred with {ErrorCount} error(s).",
            exception.GetType().FullName,
            exception.Errors.Count);
    }

    private void RecordUnhandledException(Exception exception)
    {
        _logger.LogError(
            exception,
            "An unexpected exception of type '{ExceptionType}' occurred.",
            exception.GetType().FullName);
    }

    private void AddProblemDetailsExtension(ProblemDetails problemDetails, string key, object? value)
    {
        if (problemDetails.Extensions.ContainsKey(key))
        {
            _logger.LogWarning(
                "The ProblemDetails extension key '{Key}' already exists. The existing value was overwritten while building the error response.",
                key);
        }
        problemDetails.Extensions[key] = value;
    }
}