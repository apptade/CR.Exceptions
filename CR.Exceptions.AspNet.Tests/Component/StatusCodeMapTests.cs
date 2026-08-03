using CR.Exceptions.AspNet.Mapping;
using CR.Exceptions.Tests.Shared;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class StatusCodeMapTests
{
    private const int ExpectedStatusCode = StatusCodes.Status500InternalServerError;
    private static readonly TestInternalException ExistentException = new();
    private static readonly TestUnknownException NonExistentException = new();

    [Fact]
    public void TryFind_ShouldReturn_TrueAndStatusCode_WhenExceptionExists()
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
        var result = map.TryFind(NonExistentException, out var _);

        Assert.False(result);
    }

    private static StatusCodeMap GetDefaultMap()
    {
        return new StatusCodeMapBuilder()
            .Map<TestInternalException>(ExpectedStatusCode)
            .Build();
    }
}