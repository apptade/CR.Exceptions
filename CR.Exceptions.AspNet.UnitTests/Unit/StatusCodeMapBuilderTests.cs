using CR.Exceptions.AspNet.Mapping;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.Tests.Unit;

public sealed class StatusCodeMapBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_When_DuplicateCodeRegistered()
    {
        var builder = CreateBuilder()
            .Map<ValidationException>(StatusCodes.Status400BadRequest);

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<ValidationException>(StatusCodes.Status404NotFound));
    }

    [Fact]
    public void Map_ShouldThrow_When_InvalidCodeRegistered()
    {
        var builder = CreateBuilder();

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<ValidationException>(4000));
    }

    [Fact]
    public void Build_ShouldReturn_Map_WithDefaultMappings()
    {
        var builder = CreateBuilder()
            .AddDefaultMappings();

        Assert.NotNull(() => builder.Build());
    }

    private static StatusCodeMapBuilder CreateBuilder() => new();
}