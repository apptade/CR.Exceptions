using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CR.Exceptions.AspNet;

public sealed partial class CrExceptionHandler : IExceptionHandler
{
    private static readonly CrError[] FallbackInternalErrors =
    [
        new(ErrorCodes.InternalError, "An unexpected internal error occurred.")
    ];

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
            LogResponseAlreadyStarted(_logger, exception);
            return false;
        }

        var httpStatusCode = StatusCodes.Status500InternalServerError;
        var exceptionTypeFullName = exception.GetType().FullName;

        CrError[] errors;
        string detail;

        if (exception is CrException crException)
        {
            detail = crException.Message;
            errors = crException.Errors;

            var statusCode = _options.FindHttpStatusCode(crException);

            if (statusCode is null)
            {
                LogMissingHttpStatusMapping(_logger, exception, exceptionTypeFullName);
            }
            else
            {
                httpStatusCode = statusCode.Value;
            }

            LogDomainException(_logger, exception, exceptionTypeFullName);
        }
        else
        {
            detail = "An unexpected error occurred.";
            errors = FallbackInternalErrors;

            LogUnhandledException(_logger, exception, exceptionTypeFullName);
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
            LogFailedToWriteProblemDetails(_logger, exception);

        return isWritten;
    }

    private void AddProblemDetailsExtension(ProblemDetails problemDetails, string key, object? value)
    {
        if (!problemDetails.Extensions.TryAdd(key, value))
        {
            problemDetails.Extensions[key] = value;
            LogProblemDetailsExtensionOverwritten(_logger, key);
        }
    }
}