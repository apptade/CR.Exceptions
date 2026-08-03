using CR.Exceptions.AspNet.Mapping;
using CR.Exceptions.Tests;
using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class LogLevelMapTests
{
    private const LogLevel ExpectedLogLevel = LogLevel.Warning;
    private static readonly TestInternalException ExistentException = new();
    private static readonly TestUnknownException NonExistentException = new();

    [Fact]
    public void TryFind_ShouldReturn_TrueAndLogLevel_WhenExceptionExists()
    {
        var map = GetDefaultMap();
        var result = map.TryFind(ExistentException, out var actualLevel);

        Assert.True(result);
        Assert.Equal(ExpectedLogLevel, actualLevel);
    }

    [Fact]
    public void TryFind_ShouldReturn_FalseAndDefault_WhenExceptionDoesNotExist()
    {
        var map = GetDefaultMap();
        var result = map.TryFind(NonExistentException, out var _);

        Assert.False(result);
    }

    private static LogLevelMap GetDefaultMap()
    {
        return new LogLevelMapBuilder()
            .Map<TestInternalException>(ExpectedLogLevel)
            .Build();
    }
}