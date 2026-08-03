using CR.Exceptions.Tests.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class CrExceptionHandlerTests
{
    private static readonly JsonSerializerOptions _prettyJsonOptions = new(JsonSerializerOptions.Web) { WriteIndented = true };

    private readonly ITestOutputHelper _output;

    public CrExceptionHandlerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public Task Should_Return_500_For_InternalException()
    {
        return AssertHandlerResult(
            new TestInternalException(),
            StatusCodes.Status500InternalServerError,
            canCreateActivity: true);
    }

    [Fact]
    public Task Should_Return_500_For_UnhandledException()
    {
        return AssertHandlerResult(
            new InvalidOperationException(),
            StatusCodes.Status500InternalServerError,
            canCreateActivity: true);
    }

    [Fact]
    public Task Should_Return_500_For_UnhandledException_When_ActivityIsMissing()
    {
        return AssertHandlerResult(
            new InvalidOperationException(),
            StatusCodes.Status500InternalServerError,
            canCreateActivity: false);
    }

    private async Task AssertHandlerResult(Exception exception, int expectedStatusCode, bool canCreateActivity)
    {
        using var activity = canCreateActivity
            ? new Activity("TestActivity").Start()
            : null;

        using var provider = CreateServiceProvider();
        var handler = provider.GetRequiredService<IExceptionHandler>();

        using var responseStream = new MemoryStream();
        var context = CreateContext(responseStream);

        Assert.True(await handler.TryHandleAsync(context, exception, CancellationToken.None));
        AssertHttpContext(context, expectedStatusCode);

        responseStream.Position = 0;

        var problem = await JsonSerializer.DeserializeAsync<TestProblemDetails>(responseStream, JsonSerializerOptions.Web);
        var expectedTraceId = activity?.TraceId.ToHexString() ?? context.TraceIdentifier;

        AssertProblemDetails(problem, context, expectedStatusCode, expectedTraceId);

        _output.WriteLine(JsonSerializer.Serialize(problem, options: _prettyJsonOptions));
    }

    private static void AssertHttpContext(HttpContext context, int expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, context.Response.StatusCode);
        Assert.Contains("application/problem+json", context.Response.ContentType);
    }

    private static void AssertProblemDetails(TestProblemDetails? problem, HttpContext context, int expectedStatusCode, string? expectedTraceId)
    {
        Assert.NotNull(problem);

        Assert.False(string.IsNullOrEmpty(problem.Type));
        Assert.False(string.IsNullOrEmpty(problem.Title));
        Assert.False(string.IsNullOrEmpty(problem.Detail));

        Assert.Equal(expectedStatusCode, problem.Status);
        Assert.Equal(context.Request.Path, problem.Instance);

        Assert.True(problem.Extensions.TryGetValue(ProblemDetailsExtensionNames.TraceId, out var traceId));
        Assert.Equal(expectedTraceId, traceId!.ToString());

        Assert.NotNull(problem.Errors);
        Assert.NotEmpty(problem.Errors);
    }

    private static ServiceProvider CreateServiceProvider()
    {
        return new ServiceCollection()
            .AddLogging()
            .AddCrExceptionsCore()
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

    private sealed class TestProblemDetails : ProblemDetails
    {
        public CrError[]? Errors { get; set; }
    }
}