namespace CR.Exceptions.AspNet.Tests;

internal sealed class TestInternalException : InternalException
{
    public TestInternalException() : base([new("TestInternal", "Test internal error message")])
    {
    }
}