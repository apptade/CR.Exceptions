using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class ExceptionStatusCodeOptionsTests
{
    [Fact]
    public void Should_Return_404_For_NotFoundException()
    {
        var statusCode = GetStatusCodeFor(new TestNotFoundException());
        Assert.Equal(StatusCodes.Status404NotFound, statusCode);
    }

    [Fact]
    public void Should_Return_Null_For_UnregisteredException()
    {
        var statusCode = GetStatusCodeFor(new TestUnregisteredException());
        Assert.Null(statusCode);
    }

    private static int? GetStatusCodeFor(CrException exception)
    {
        return new ExceptionStatusCodeOptions()
            .AddDefaultMappings()
            .FindHttpStatusCode(exception);
    }

    private sealed class TestUnregisteredException : CrException
    {
        public TestUnregisteredException() : base([new("TestUnregistered", "Test message")], "Unregistered")
        {
        }
    }
}