namespace CR.Exceptions.AspNet.UnitTests;

public sealed class TestConflictException : ConflictException
{
    public TestConflictException() : base(
        [new CrError("TestConflict", "Test Conflict")],
        "Test Message Conflict")
    { }
}