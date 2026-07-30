namespace CR.Exceptions.AspNet.UnitTests;

internal sealed class TestNotFoundException : NotFoundException
{
    public TestNotFoundException() : base([new("TestNotFound", "Test Entity not found")])
    {
    }
}