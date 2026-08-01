using CR.Exceptions.Mapping;

namespace CR.Exceptions.Tests.Component;

public sealed class ExceptionFactoryTests
{
    [Fact]
    public void TryCreate_ShouldReturn_Exception_WhenCodeExists()
    {
        const string errorCode = "TestError";
        const string registrationCode = "test_error";

        var errorRegistration = new ErrorRegistration(registrationCode, [new(errorCode, "Something went wrong.")]);

        var factory = new ExceptionFactoryBuilder()
            .Add(new(errorRegistration, errors => new TestException(errors)))
            .Build();

        var result = factory.TryCreate(registrationCode, out var exception);

        Assert.True(result);
        Assert.NotNull(exception);

        var typedException = Assert.IsType<TestException>(exception);
        var singleError = Assert.Single(typedException.Errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryCreate_ShouldReturn_False_WhenCodeNotExist()
    {
        var factory = new ExceptionFactoryBuilder()
            .Add(new(new("?", [new("?", "?")]), errors => new TestException(errors)))
            .Build();

        var result = factory.TryCreate("non_existent_code", out var exception);

        Assert.False(result);
        Assert.Null(exception);
    }
}