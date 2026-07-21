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
    public async Task Should_Return_404_For_NotFoundException()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddCrExceptionHandler();

        var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<IExceptionHandler>();

        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };
        var result = await handler.TryHandleAsync(context, new TestNotFoundException(), CancellationToken.None);

        context.Response.Body.Position = 0;
        var jsonNode = await JsonNode.ParseAsync(context.Response.Body);
        _output.WriteLine(jsonNode?.ToJsonString(_jsonSerializerOptions));

        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}