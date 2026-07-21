using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit.Abstractions;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class CrExceptionHandlerTests
{
    private readonly ITestOutputHelper _output;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CrExceptionHandlerTests(ITestOutputHelper output)
    {
        _output = output;
        _jsonSerializerOptions = new() { WriteIndented = true };
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

    private async Task LogResponseBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        var jsonNode = await JsonNode.ParseAsync(context.Response.Body);
        _output.WriteLine(jsonNode?.ToJsonString(_jsonSerializerOptions));
    }
}