using CR.Exceptions.Mapping;

namespace CR.Exceptions.UnitTests;

public sealed class ExceptionFactoryTests
{
    [Fact]
    public void Create_ShouldReturnRegisteredException()
    {
        var errorCode = "TestError";
        var registrationCode = "test_error";

        var registration = new ErrorRegistration(
            registrationCode,
            [new CrError(errorCode, "Something went wrong.")]);

        var exceptionRegistration = new ExceptionRegistration(
            registration,
            errors => new TestException(errors));

        var factory = new ExceptionFactoryBuilder()
            .Add(exceptionRegistration)
            .Build();

        var exception = factory.Create(registrationCode);

        Assert.IsType<TestException>(exception);
        var singleError = Assert.Single(exception.Errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryCreate_ShouldReturnFalse_WhenCodeDoesNotExist()
    {
        var factory = new ExceptionFactoryBuilder()
            .Build();

        var result = factory.TryCreate(
            "unknown",
            out var exception);

        Assert.False(result);
        Assert.Null(exception);
    }
}