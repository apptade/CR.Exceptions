using CR.Exceptions.Mapping;

namespace CR.Exceptions.UnitTests;

public sealed class ExceptionFactoryTests
{
    [Fact]
    public void Create_ShouldReturnRegisteredException()
    {
        var errorCode = "TestError";
        var registrationCode = "test_error";

        var errorRegistration = new ErrorRegistration(registrationCode, [new(errorCode, "Something went wrong.")]);
        var exceptionRegistration = new ExceptionRegistration(errorRegistration, errors => new TestException(errors));

        var factory = new ExceptionFactoryBuilder()
            .Add(exceptionRegistration)
            .Build();

        var exception = factory.Create(registrationCode);

        Assert.IsType<TestException>(exception);
        var singleError = Assert.Single(exception.Errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryCreate_ShouldReturnFalse_WhenCodeNotExist()
    {
        var factory = new ExceptionFactoryBuilder().Build();
        var result = factory.TryCreate("random", out var exception);

        Assert.False(result);
        Assert.Null(exception);
    }

    [Fact]
    public void Build_ShouldThrow_WhenDuplicateCodesRegistered()
    {
        var errorRegistration = new ErrorRegistration("duplicate", [new("code", "message")]);
        var exceptionRegistration = new ExceptionRegistration(errorRegistration, errors => new TestException(errors));

        var builder = new ExceptionFactoryBuilder()
            .Add(exceptionRegistration)
            .Add(exceptionRegistration);

        Assert.Throws<InvalidOperationException>(builder.Build);
    }
}