namespace Cr.Exceptions.Tests.Unit;

public sealed class ExceptionTranslatorBuilderTests
{
    [Fact]
    public void Map_ShouldThrow_WhenDuplicateRegistered()
    {
        var builder = new ExceptionTranslatorBuilder()
            .Map<InternalException>(ex => new UnprocessableException(ex));

        Assert.ThrowsAny<ArgumentException>(() => builder.Map<InternalException>(ex => new ConflictException(ex)));
    }
}