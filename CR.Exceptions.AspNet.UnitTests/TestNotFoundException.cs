namespace CR.Exceptions.AspNet.UnitTests;

public sealed class TestNotFoundException : NotFoundException
{
    public TestNotFoundException() : base(
        [new("TestNotFound", "Test Entity not found")],
        "Test Message Entity not found")
    { }
}