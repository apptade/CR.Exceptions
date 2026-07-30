using CR.Exceptions.Mapping;

namespace CR.Exceptions.UnitTests;

public sealed class ErrorMapTests
{
    [Fact]
    public void TryGet_ShouldReturnErrors_WhenCodeExists()
    {
        var errorCode = "InvalidGrant";
        var registrationCode = "invalid_grant";

        var registration = new ErrorRegistration(registrationCode, [new(errorCode, "Invalid username or password.")]);

        var map = new ErrorMapBuilder()
            .Add(registration)
            .Build();

        var result = map.TryGet(registrationCode, out var errors);

        Assert.True(result);
        var singleError = Assert.Single(errors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryGet_ShouldReturnFalse_WhenCodeNotExist()
    {
        var map = new ErrorMapBuilder().Build();
        var result = map.TryGet("random", out var errors);

        Assert.False(result);
        Assert.True(errors.IsDefaultOrEmpty);
    }

    [Fact]
    public void Build_ShouldThrow_WhenDuplicateCodesRegistered()
    {
        var errorRegistration = new ErrorRegistration("duplicate", [new("code", "message")]);

        var builder = new ErrorMapBuilder()
            .Add(errorRegistration)
            .Add(errorRegistration);

        Assert.Throws<InvalidOperationException>(builder.Build);
    }
}