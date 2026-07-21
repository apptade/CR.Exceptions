using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.UnitTests;

public sealed class ExceptionMappingOptionsTests
{
    [Theory]
    [InlineData(typeof(TestNotFoundException), StatusCodes.Status404NotFound)]
    [InlineData(typeof(TestConflictException), StatusCodes.Status409Conflict)]
    public void Should_Return_Correct_Status_Code(Type exceptionType, int expectedStatusCode)
    {
        var options = new ExceptionMappingOptions().AddDefaultMappings();
        var exception = (CrException)Activator.CreateInstance(exceptionType)!;
        var statusCode = options.FindHttpStatusCode(exception);

        statusCode.Should()
            .Be(expectedStatusCode);
    }
}