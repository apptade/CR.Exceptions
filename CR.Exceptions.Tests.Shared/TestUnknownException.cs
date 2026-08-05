using System.Collections.Immutable;

namespace CR.Exceptions.Tests.Shared;

public sealed class TestUnknownException : CrException
{
    private static readonly ImmutableArray<CrError> _errors = [new("TestUnknownCode", "TestUnknownMessage")];

    public TestUnknownException(Exception? innerException = null) : base(_errors, "Test unknown exception message", innerException) { }
}