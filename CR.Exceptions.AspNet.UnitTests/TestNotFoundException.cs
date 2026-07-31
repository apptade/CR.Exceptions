namespace CR.Exceptions.AspNet.Tests;

internal sealed class TestNotFoundException : NotFoundException
{
    public TestNotFoundException() : base([new("TestNotFound", "Test entity not found error message")])
    {
    }
}