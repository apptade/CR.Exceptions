namespace CR.Exceptions.AspNet.Tests;

internal sealed class TestUnregisteredException : CrException
{
    public TestUnregisteredException() : base([new("TestUnregistered", "Test error message")], "Unregistered detail")
    {
    }
}