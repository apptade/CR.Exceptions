using Microsoft.AspNetCore.Http;

namespace Cr.Exceptions.AspNetCore.Tests.Component;

public sealed class StatusCodeMapTests
{
    private const int ExpectedStatusCode = StatusCodes.Status500InternalServerError;
    private static readonly InternalException ExistentException = new();
    private static readonly CrException NonExistentException = new();

    [Fact]
    public void TryFind_ShouldReturn_TrueAndCode_WhenExceptionExists()
    {
        var map = GetDefaultMap();
        var result = map.TryFind(ExistentException, out var actualCode);

        Assert.True(result);
        Assert.Equal(ExpectedStatusCode, actualCode);
    }

    [Fact]
    public void TryFind_ShouldReturn_FalseAndDefault_WhenExceptionDoesNotExist()
    {
        var map = GetDefaultMap();
        var result = map.TryFind(NonExistentException, out var code);

        Assert.False(result);
        Assert.Equal(default, code);
    }

    private static StatusCodeMap GetDefaultMap()
    {
        return new StatusCodeMapBuilder()
            .Map<InternalException>(ExpectedStatusCode)
            .Build();
    }
}