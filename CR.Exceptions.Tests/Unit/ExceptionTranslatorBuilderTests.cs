using CR.Exceptions.Tests.Shared;

namespace CR.Exceptions.Tests.Unit;

public sealed class ExceptionTranslatorBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_WhenDuplicateRegistered()
    {
        var builder = new ExceptionTranslatorBuilder()
            .Map<TestInternalException>(ex => new TestUnknownException(ex));

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<TestInternalException>(ex => new TestUnknownException(ex)));
    }
}