using Microsoft.Extensions.Logging;

namespace Cr.Exceptions.AspNetCore.Tests.Component;

public sealed class LogLevelMapTests
{
    private const LogLevel ExpectedLogLevel = LogLevel.Warning;
    private static readonly InternalException ExistentException = new();
    private static readonly CrException NonExistentException = new();

    [Fact]
    public void TryFind_ShouldReturn_TrueAndLevel_WhenExceptionExists()
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
        var result = map.TryFind(NonExistentException, out var level);

        Assert.False(result);
        Assert.Equal(default, level);
    }

    private static LogLevelMap GetDefaultMap()
    {
        return new LogLevelMapBuilder()
            .Map<InternalException>(ExpectedLogLevel)
            .Build();
    }
}