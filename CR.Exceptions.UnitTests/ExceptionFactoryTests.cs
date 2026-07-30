using CR.Exceptions.Mapping;

namespace CR.Exceptions.UnitTests;

public sealed class ExceptionFactoryTests
{
    [Fact]
    public void TryCreate_ShouldReturnException_WhenCodeExists()
    {
        const string errorCode = "TestError";
        const string registrationCode = "test_error";

        var errorRegistration = new ErrorRegistration(registrationCode, [new(errorCode, "Something went wrong.")]);
        var exceptionRegistration = new ExceptionRegistration(errorRegistration, errors => new TestException(errors));

        var factory = new ExceptionFactoryBuilder()
            .Add(exceptionRegistration)
            .Build();

        var result = factory.TryCreate(registrationCode, out var exception);

        Assert.True(result);
        Assert.NotNull(exception);

        var typedException = Assert.IsType<TestException>(exception);
        var singleError = Assert.Single(typedException.Errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryCreate_ShouldReturnFalse_WhenCodeNotExist()
    {
        var factory = new ExceptionFactoryBuilder().Build();

        var result = factory.TryCreate("non_existent_code", out var exception);

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