namespace CR.Exceptions.AspNet.UnitTests;

public sealed class TestUnregisteredException : CrException
{
    public TestUnregisteredException() : base([new("TestUnregistered", "Test message")], "Unregistered")
    {
    }
}