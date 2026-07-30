using CR.Exceptions.Mapping;
using System.Collections.Immutable;

namespace CR.Exceptions.UnitTests;

public sealed class ErrorMapTests
{
    [Fact]
    public void TryGet_ShouldReturnErrors_WhenCodeExists()
    {
        var errorCode = "InvalidGrant";
        var registrationCode = "invalid_grant";

        ImmutableArray<CrError> errors =
        [
            new CrError(errorCode, "Invalid username or password.")
        ];

        var registration = new ErrorRegistration(registrationCode, errors);

        var map = new ErrorMapBuilder()
            .Add(registration)
            .Build();

        var result = map.TryGet(
            registrationCode,
            out var resolvedErrors);

        Assert.True(result);
        var singleError = Assert.Single(resolvedErrors);
        Assert.Equal(errorCode, singleError.Code);
    }

    [Fact]
    public void TryGet_ShouldReturnFalse_WhenCodeDoesNotExist()
    {
        var map = new ErrorMapBuilder()
            .Build();

        var result = map.TryGet(
            "unknown",
            out var errors);

        Assert.False(result);
        Assert.True(errors.IsEmpty);
    }

    [Fact]
    public void Build_ShouldThrow_WhenDuplicateCodesRegistered()
    {
        var first = new ErrorRegistration(
            "duplicate",
            [new("Code.One", "First")]);

        var second = new ErrorRegistration(
            "duplicate",
            [new("Code.Two", "Second")]);

        var builder = new ErrorMapBuilder()
            .Add(first)
            .Add(second);

        Assert.Throws<InvalidOperationException>(builder.Build);
    }
}