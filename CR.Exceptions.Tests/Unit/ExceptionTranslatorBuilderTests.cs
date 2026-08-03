using CR.Exceptions.Mapping;

namespace CR.Exceptions.Tests.Unit;

public sealed class ExceptionTranslatorBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_WhenDuplicateRegistered()
    {
        var builder = new ExceptionTranslatorBuilder()
            .Map<TestInternalException>(() => new TestUnknownException());

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<TestInternalException>(() => new TestUnknownException()));
    }
}