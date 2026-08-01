using CR.Exceptions.Mapping;

namespace CR.Exceptions.Tests.Unit;

public sealed class ExceptionFactoryBuilderTests
{
    [Fact]
    public void Add_ShouldThrow_WhenDuplicateRegistered()
    {
        var errorRegistration = new ErrorRegistration("duplicate", [new("code", "message")]);
        var exceptionRegistration = new ExceptionRegistration(errorRegistration, errors => new TestException(errors));

        var builder = new ExceptionFactoryBuilder()
            .Add(exceptionRegistration);

        Assert.ThrowsAny<ArgumentException>(() => builder.Add(exceptionRegistration));
    }
}