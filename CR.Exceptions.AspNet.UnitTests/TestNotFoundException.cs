namespace CR.Exceptions.AspNet.UnitTests;

public sealed class TestNotFoundException : NotFoundException
{
    public TestNotFoundException() : base(
        [
            new Error("TestNotFound", "Entity not found")
        ],
        "Entity not found")
    { }
}