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
        string errorCode, errorMessage;

        if (exception is CrException crException)
        {
            errorCode = crException.Code;
            errorMessage = crException.Message;

            var statusCode = _options.FindHttpStatusCode(crException);

            if (statusCode is null)
                _logger.LogWarning(
                    crException,
                    "No HTTP status mapping found for exception type '{ExceptionType}'. Using 500 Internal Server Error.",
                    crException.GetType().FullName);
            else
                httpStatusCode = statusCode.Value;

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug(crException, "Domain error occurred. Code: '{Code}'", errorCode);
        }
        else
        {
            errorCode = ExceptionCodes.InternalError;
            errorMessage = "An unexpected error occurred.";

            _logger.LogError(exception, "An unexpected error occurred. Code: '{Code}'", errorCode);
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
                Detail = errorMessage,
                Instance = httpContext.Request.Path,
            }
        };

        AddProblemDetailsExtension(problemDetailsContext.ProblemDetails, "code", errorCode);
        AddProblemDetailsExtension(problemDetailsContext.ProblemDetails, "traceId", traceId);

        httpContext.Response.StatusCode = httpStatusCode;

        return await _problemDetailsService.TryWriteAsync(problemDetailsContext);
    }

    private void AddProblemDetailsExtension(ProblemDetails problemDetails, string key, object? value)
    {
        if (problemDetails.Extensions.ContainsKey(key))
        {
            _logger.LogWarning(
                "The ProblemDetails extension key '{Key}' already exists. The existing value was overwritten while building the error response.", key);
        }
        problemDetails.Extensions[key] = value;
    }
}