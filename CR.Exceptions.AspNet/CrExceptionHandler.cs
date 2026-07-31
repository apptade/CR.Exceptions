using CR.Exceptions.AspNet.Mapping;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace CR.Exceptions.AspNet;

public sealed partial class CrExceptionHandler : IExceptionHandler
{
    private static readonly ImmutableArray<CrError> DefaultInternalErrors =
        [new("InternalError", "An unexpected internal error occurred.")];

    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<CrExceptionHandler> _logger;

    private readonly StatusCodeMap _statusCodeMap;
    private readonly LogLevelMap _logLevelMap; 

    public CrExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<CrExceptionHandler> logger,
        StatusCodeMap statusCodeMap,
        LogLevelMap logLevelMap)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;

        _statusCodeMap = statusCodeMap;
        _logLevelMap = logLevelMap;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (httpContext.Response.HasStarted)
        {
            LogResponseAlreadyStarted(_logger, exception);
            return false;
        }

        var statusCode = StatusCodes.Status500InternalServerError;
        var exceptionType = exception.GetType();
        var exceptionTypeName = exceptionType.FullName ?? exceptionType.Name;

        var errors = DefaultInternalErrors;
        var detail = "An unexpected error occurred.";

        if (exception is CrException crException)
        {
            detail = crException.Message;
            errors = crException.Errors;

            if (_statusCodeMap.TryFind(crException, out var code))
            {
                statusCode = code;
            }
            else
            {
                LogMissingHttpStatusMapping(_logger, exception, exceptionTypeName);
            }

            var logLevel = _logLevelMap.TryFind(crException, out var level) ? level : LogLevel.Debug;
            LogApplicationException(_logger, logLevel, exception, exceptionTypeName);
        }
        else
        {
            LogUnhandledException(_logger, exception, exceptionTypeName);
        }

        httpContext.Response.StatusCode = statusCode;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = statusCode,
                Detail = detail,
                Instance = httpContext.Request.Path
            },
        };
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