using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class ExceptionMappingOptionsTests
{
    [Fact]
    public void Should_Return_404_Status_Code_For_NotFoundException()
    {
        var options = new ExceptionMappingOptions().AddDefaultMappings();
        var exception = new TestNotFoundException();

        var statusCode = options.FindHttpStatusCode(exception);

        statusCode.Should()
            .Be(StatusCodes.Status404NotFound);
    }
}