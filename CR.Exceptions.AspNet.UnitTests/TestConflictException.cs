namespace CR.Exceptions.AspNet.UnitTests;

public sealed class TestConflictException : ConflictException
{
    public TestConflictException() : base([new("TestConflict", "Test Conflict")])
    {
    }
}