using CR.Exceptions.AspNet.Mapping;
using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class LogLevelMapTests
{
    [Fact]
    public void TryFind_ShouldReturn_ErrorLevel_For_InternalException()
    {
        var map = CreateMap();
        var result = map.TryFind(new TestInternalException(), out var code);

        Assert.True(result);
        Assert.Equal(LogLevel.Error, code);
    }

    [Fact]
    public void TryFind_ShouldReturn_False_For_UnregisteredException()
    {
        var map = CreateMap();

        Assert.False(map.TryFind(new TestUnregisteredException(), out var _));
    }

    private LogLevelMap CreateMap()
    {
        return new LogLevelMapBuilder()
            .AddDefaultMappings()
            .Build();
    }
}