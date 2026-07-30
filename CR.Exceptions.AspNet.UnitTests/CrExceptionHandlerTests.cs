using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class CrExceptionHandlerTests
{
    private static readonly JsonSerializerOptions _prettyJsonOptions = new(JsonSerializerOptions.Web) { WriteIndented = true };

    private readonly ITestOutputHelper _output;

    public CrExceptionHandlerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public Task Should_Return_404_For_NotFoundException()
    {
        return AssertHandlerResult(new TestNotFoundException(), StatusCodes.Status404NotFound);
    }

    [Fact]
    public Task Should_Return_500_For_UnhandledException()
    {
        return AssertHandlerResult(new TestUnregisteredException(), StatusCodes.Status500InternalServerError);
    }

    private async Task AssertHandlerResult(Exception exception, int expectedStatusCode)
    {
        using var activity = new Activity("TestActivity").Start();
        using var provider = CreateServiceProvider();
        var handler = provider.GetRequiredService<IExceptionHandler>();

        using var responseStream = new MemoryStream();
        var context = CreateContext(responseStream);

        var isHandled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(isHandled);
        Assert.Equal(expectedStatusCode, context.Response.StatusCode);
        Assert.Contains("application/problem+json", context.Response.ContentType);

        responseStream.Position = 0;
        var problem = await JsonSerializer.DeserializeAsync<CustomProblemDetails>(responseStream, JsonSerializerOptions.Web);

        AssertProblemDetails(problem, context, expectedStatusCode, activity.TraceId.ToHexString());
    }

    private static ServiceProvider CreateServiceProvider()
    {
        return new ServiceCollection()
            .AddLogging()
            .AddCrExceptionHandler()
            .BuildServiceProvider();
    }

    private static DefaultHttpContext CreateContext(MemoryStream responseStream)
    {
        return new DefaultHttpContext
        {
            Request = { Path = "/api/test" },
            Response = { Body = responseStream }
        };
    }

    private void AssertProblemDetails(CustomProblemDetails? problem, HttpContext context, int expectedStatusCode, string? expectedTraceId)
    {
        Assert.NotNull(problem);
        _output.WriteLine(JsonSerializer.Serialize(problem, options: _prettyJsonOptions));

        Assert.False(string.IsNullOrEmpty(problem.Type));
        Assert.False(string.IsNullOrEmpty(problem.Title));
        Assert.False(string.IsNullOrEmpty(problem.Detail));

        Assert.Equal(expectedStatusCode, problem.Status);
        Assert.Equal(context.Request.Path, problem.Instance);

        var actualTraceId = problem.Extensions.TryGetValue(ProblemDetailsExtensionNames.TraceId, out var id) ? id?.ToString() : null;
        Assert.Equal(expectedTraceId, actualTraceId);

        Assert.NotNull(problem.Errors);
        Assert.NotEmpty(problem.Errors);
    }

    private sealed class CustomProblemDetails : ProblemDetails
    {
        public CrError[]? Errors { get; set; }
    }
}