using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class ExceptionStatusCodeOptionsTests
{
    [Fact]
    public void Should_Return_404_Status_Code_For_NotFoundException()
    {
        var options = new ExceptionStatusCodeOptions().AddDefaultMappings();
        var exception = new TestNotFoundException();
        var statusCode = options.FindHttpStatusCode(exception);

        Assert.Equal(StatusCodes.Status404NotFound, statusCode);
    }
}