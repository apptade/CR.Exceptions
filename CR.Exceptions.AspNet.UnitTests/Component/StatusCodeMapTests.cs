using CR.Exceptions.AspNet.Mapping;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.Tests.Component;

public sealed class StatusCodeMapTests
{
    [Fact]
    public void TryFind_ShouldReturn_404_For_NotFoundException()
    {
        var map = CreateMap();
        var result = map.TryFind(new TestNotFoundException(), out var code);

        Assert.True(result);
        Assert.Equal(StatusCodes.Status404NotFound, code);
    }

    [Fact]
    public void TryFind_ShouldReturn_False_For_UnregisteredException()
    {
        var map = CreateMap();

        Assert.False(map.TryFind(new TestUnregisteredException(), out var _));
    }

    private StatusCodeMap CreateMap()
    {
        return new StatusCodeMapBuilder()
            .AddDefaultMappings()
            .Build();
    }
}