using CR.Exceptions.AspNet.Mapping;
using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class LogLevelMapTests
{
    [Fact]
    public void TryFind_ShouldReturn_Level_For_NotFoundException()
    {
        var level = LogLevel.Warning;
        var map = CreateMap(builder => builder.Map<NotFoundException>(level));

        var result = map.TryFind(new TestNotFoundException(), out var actualLevel);

        Assert.True(result);
        Assert.Equal(level, actualLevel);
    }

    [Fact]
    public void TryFind_ShouldReturn_False_For_UnregisteredException()
    {
        var map = CreateMap();

        Assert.False(map.TryFind(new TestUnregisteredException(), out var _));
    }

    private static LogLevelMap CreateMap(Action<LogLevelMapBuilder>? configurator = null)
    {
        var builder = new LogLevelMapBuilder();
        configurator?.Invoke(builder);

        return builder.Build();
    }
}