using CR.Exceptions.Mapping;

namespace CR.Exceptions.Tests.Unit;

public sealed class ErrorMapBuilderTests
{
    [Fact]
    public void Add_ShouldThrow_WhenDuplicateRegistered()
    {
        var errorRegistration = new ErrorRegistration("duplicate", [new("code", "message")]);

        var builder = new ErrorMapBuilder()
            .Add(errorRegistration);

        Assert.ThrowsAny<ArgumentException>(() => builder.Add(errorRegistration));
    }
}