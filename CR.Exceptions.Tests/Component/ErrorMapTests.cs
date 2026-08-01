using CR.Exceptions.Mapping;

namespace CR.Exceptions.Tests.Component;

public sealed class ErrorMapTests
{
    [Fact]
    public void TryGet_ShouldReturn_Errors_WhenCodeExists()
    {
        const string errorCode = "InvalidGrant";
        const string registrationCode = "invalid_grant";

        var map = new ErrorMapBuilder()
            .Add(new(registrationCode, [new(errorCode, "Invalid username or password.")]))
            .Build();

        var result = map.TryGet(registrationCode, out var errors);

        Assert.True(result);
        var singleError = Assert.Single(errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryGet_ShouldReturn_False_WhenCodeNotExist()
    {
        var map = new ErrorMapBuilder()
            .Add(new("?", [new("?", "?")]))
            .Build();

        var result = map.TryGet("non_existent_code", out var errors);

        Assert.False(result);
        Assert.True(errors.IsDefaultOrEmpty);
    }
}