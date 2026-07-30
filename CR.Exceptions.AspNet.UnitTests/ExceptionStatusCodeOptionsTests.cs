using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class ExceptionStatusCodeOptionsTests
{
    [Fact]
    public void Should_Return_404_For_NotFoundException()
    {
        var options = new ExceptionStatusCodeOptions().AddDefaultMappings();
        var statusCode = options.FindHttpStatusCode(new TestNotFoundException());

        Assert.Equal(StatusCodes.Status404NotFound, statusCode);
    }

    [Fact]
    public void Should_Return_Null_For_UnregisteredException()
    {
        var options = new ExceptionStatusCodeOptions().AddDefaultMappings();
        var statusCode = options.FindHttpStatusCode(new TestUnregisteredException());

        Assert.Null(statusCode);
    }
}