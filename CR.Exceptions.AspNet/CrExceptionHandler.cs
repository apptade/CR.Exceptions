using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace CR.Exceptions.AspNet;

public sealed partial class CrExceptionHandler : IExceptionHandler
{
    private static readonly ImmutableArray<CrError> DefaultInternalErrors =
        [new("InternalError", "An unexpected internal error occurred.")];

    private readonly IProblemDetailsService _problemDetailsService;
    private readonly CrExceptionOptions _options;
    private readonly ILogger<CrExceptionHandler> _logger;

    public CrExceptionHandler(
        IProblemDetailsService problemDetailsService,
        IOptions<CrExceptionOptions> options,
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
        var exceptionType = exception.GetType();
        var exceptionTypeName = exceptionType.FullName ?? exceptionType.Name;

        ImmutableArray<CrError> errors;
        string detail;

        if (exception is CrException crException)
        {
            detail = crException.Message;
            errors = crException.Errors;

            var statusCode = _options.StatusCodes.FindHttpStatusCode(crException);

            if (statusCode is null)
            {
                LogMissingHttpStatusMapping(_logger, exception, exceptionTypeName);
            }
            else
            {
                httpStatusCode = statusCode.Value;
            }

            LogApplicationException(_logger, exception, exceptionTypeName);
        }
        else
        {
            detail = "An unexpected error occurred.";
            errors = DefaultInternalErrors;

            LogUnhandledException(_logger, exception, exceptionTypeName);
        }

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = httpStatusCode,
                Detail = detail,
                Instance = httpContext.Request.Path
            },
        };

        httpContext.Response.StatusCode = httpStatusCode;

        AddProblemDetailsExtension(problemDetailsContext.ProblemDetails, ProblemDetailsExtensionNames.Errors, errors);

        var isWritten = await _problemDetailsService.TryWriteAsync(problemDetailsContext);

        if (!isWritten)
        {
            LogFailedToWriteProblemDetails(_logger, exception);
        }

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