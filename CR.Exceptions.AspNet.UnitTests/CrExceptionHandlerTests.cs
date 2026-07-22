using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Xunit.Abstractions;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class CrExceptionHandlerTests
{
    private readonly ITestOutputHelper _output;
    private readonly JsonSerializerOptions _jsonOptions;

    public CrExceptionHandlerTests(ITestOutputHelper output)
    {
        _output = output;
        _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public Task Should_Return_404_For_NotFoundException()
    {
        return ShouldReturnStatusCode(
            new TestNotFoundException(),
            StatusCodes.Status404NotFound);
    }

    [Fact]
    public Task Should_Return_500_For_UnhandledException()
    {
        return ShouldReturnStatusCode(
            new Exception("Something went wrong"),
            StatusCodes.Status500InternalServerError);
    }

    private async Task ShouldReturnStatusCode(Exception exception, int expectedStatusCode)
    {
        using var provider = CreateServiceProvider();

        var handler = provider.GetRequiredService<IExceptionHandler>();
        var context = CreateContext();

        var result = await handler.TryHandleAsync(
            context,
            exception,
            CancellationToken.None);

        await LogResponseBody(context);

        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(expectedStatusCode);

        var problem = await DeserializeProblemDetails(context);
        AssertProblemDetails(problem!, context, expectedStatusCode);
    }

    private static ServiceProvider CreateServiceProvider()
    {
        return new ServiceCollection()
            .AddLogging()
            .AddCrExceptionHandler()
            .BuildServiceProvider();
    }

    private static DefaultHttpContext CreateContext()
    {
        return new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
    }

    private async Task<ProblemDetailsResponse> DeserializeProblemDetails(HttpContext context)
    {
        context.Response.Body.Position = 0;

        return (await JsonSerializer.DeserializeAsync<ProblemDetailsResponse>(
            context.Response.Body, _jsonOptions))!;
    }

    private async Task LogResponseBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        var jsonNode = await JsonNode.ParseAsync(context.Response.Body);
        _output.WriteLine(jsonNode?.ToJsonString(_jsonOptions));
    }

    private static void AssertProblemDetails(ProblemDetailsResponse problem, HttpContext context, int expectedStatusCode)
    {
        problem.Should().NotBeNull();

        problem.Type.Should().NotBeNullOrWhiteSpace();
        problem.Title.Should().NotBeNullOrWhiteSpace();
        problem.Detail.Should().NotBeNullOrWhiteSpace();

        problem.Status.Should().Be(expectedStatusCode);
        problem.Instance.Should().Be(context.Request.Path);

        problem.TraceId.Should().NotBeNullOrWhiteSpace();
        problem.Errors.Should().NotBeNull().And.NotBeEmpty();
    }

    private sealed class ProblemDetailsResponse
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }

        [JsonPropertyName(ProblemDetailsExtensionNames.TraceId)]
        public string? TraceId { get; set; }

        [JsonPropertyName(ProblemDetailsExtensionNames.Errors)]
        public CrError[]? Errors { get; set; }
    }
}