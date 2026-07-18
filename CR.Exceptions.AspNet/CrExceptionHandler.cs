using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace CR.Exceptions.AspNet;

public sealed class CrExceptionHandler : IExceptionHandler
{
    private readonly CrExceptionOptions _options;
    private readonly ILogger<CrExceptionHandler> _logger;

    public CrExceptionHandler(IOptions<CrExceptionOptions> options, ILogger<CrExceptionHandler> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var crException = exception as CrException;
        var isCrException = crException != null;

        if (!_options.TryGetStatusCode(exception, out var statusCode))
        {
            statusCode = HttpStatusCode.InternalServerError;
        }

        if (!isCrException)
        {
            _logger.LogError(exception, "An unhandled exception occurred during the request.");
        }

        httpContext.Response.StatusCode = (int)statusCode;
        var response = new
        {
            code = crException?.Code ?? "INTERNAL_ERROR",
            message = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}