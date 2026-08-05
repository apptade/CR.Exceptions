using CR.Exceptions.Tests.Shared;

namespace CR.Exceptions.Tests.Unit;

public sealed class ExceptionFactoryBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_WhenDuplicateRegistered()
    {
        const string code = "duplicate";

        var builder = new ExceptionFactoryBuilder()
            .Map(code, () => new TestUnknownException());

        Assert.ThrowsAny<ArgumentException>(() => builder.Map(code, () => new TestUnknownException()));
    }
}