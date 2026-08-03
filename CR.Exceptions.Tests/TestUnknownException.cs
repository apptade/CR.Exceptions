using System.Collections.Immutable;

namespace CR.Exceptions.Tests;

public sealed class TestUnknownException : CrException
{
    private static readonly ImmutableArray<CrError> _errors = [new("TestUnknownCode", "TestUnknownMessage")];

    public TestUnknownException() : base(_errors, "Test unknown exception message") { }
}