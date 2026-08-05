using Microsoft.Extensions.Logging;

namespace CR.Exceptions.AspNet.Tests.Unit;

public sealed class LogLevelMapBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_When_DuplicateLevelRegistered()
    {
        var builder = CreateBuilder()
            .Map<InternalException>(LogLevel.Error);

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<InternalException>(LogLevel.Warning));
    }

    [Fact]
    public void Map_ShouldThrow_When_InvalidLevelRegistered()
    {
        var builder = CreateBuilder();

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<ConflictException>((LogLevel)4000));
    }

    [Fact]
    public void Map_ShouldThrow_When_NoneLevelRegistered()
    {
        var builder = CreateBuilder();

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<ConflictException>(LogLevel.None));
    }

    [Fact]
    public void Build_ShouldReturn_Map_WithDefaultMappings()
    {
        var builder = CreateBuilder()
            .AddDefaultMappings();

        Assert.NotNull(() => builder.Build());
    }

    private static LogLevelMapBuilder CreateBuilder() => new();
}