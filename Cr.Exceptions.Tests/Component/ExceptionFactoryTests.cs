namespace Cr.Exceptions.Tests.Component;

public sealed class ExceptionFactoryTests
{
    private const string ExistentCode = "Test";
    private const string NonExistentCode = "non_existent_code";

    [Fact]
    public void TryCreate_ShouldReturn_TrueAndException_WhenCodeExists()
    {
        var factory = GetDefaultFactory();
        var result = factory.TryCreate(ExistentCode, out var exception);

        Assert.True(result);
        Assert.NotNull(exception);
        Assert.IsType<InternalException>(exception);
    }

    [Fact]
    public void TryCreate_ShouldReturn_FalseAndNull_WhenCodeDoesNotExist()
    {
        var factory = GetDefaultFactory();
        var result = factory.TryCreate(NonExistentCode, out var exception);

        Assert.False(result);
        Assert.Null(exception);
    }

    [Fact]
    public void Create_ShouldReturn_Exception_WhenCodeExists()
    {
        var factory = GetDefaultFactory();
        var exception = factory.Create(ExistentCode);

        Assert.NotNull(exception);
        Assert.IsType<InternalException>(exception);
    }

    [Fact]
    public void Create_ShouldThrow_WhenCodeDoesNotExist()
    {
        var factory = GetDefaultFactory();

        Assert.Throws<KeyNotFoundException>(() => factory.Create(NonExistentCode));
    }

    private static ExceptionFactory GetDefaultFactory()
    {
        return new ExceptionFactoryBuilder()
            .Map(ExistentCode, () => new InternalException())
            .Build();
    }
}