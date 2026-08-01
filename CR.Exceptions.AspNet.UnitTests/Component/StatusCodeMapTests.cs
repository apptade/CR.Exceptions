using CR.Exceptions.AspNet.Mapping;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class StatusCodeMapTests
{
    [Fact]
    public void TryFind_ShouldReturn_404_For_NotFoundException()
    {
        var code = StatusCodes.Status404NotFound;
        var map = CreateMap(builder => builder.Map<NotFoundException>(code));

        var result = map.TryFind(new TestNotFoundException(), out var actualCode);

        Assert.True(result);
        Assert.Equal(code, actualCode);
    }

    [Fact]
    public void TryFind_ShouldReturn_False_For_UnregisteredException()
    {
        var map = CreateMap();

        Assert.False(map.TryFind(new TestUnregisteredException(), out var _));
    }

    private static StatusCodeMap CreateMap(Action<StatusCodeMapBuilder>? configurator = null)
    {
        var builder = new StatusCodeMapBuilder();
        configurator?.Invoke(builder);

        return builder.Build();
    }
}